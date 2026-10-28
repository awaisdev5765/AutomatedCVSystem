using AutomatedCVSystem.Core.DTOs;
using AutomatedCVSystem.Core.Interfaces;
using AutomatedCVSystem.Core.Models;
using AutomatedCVSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text.Json;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace AutomatedCVSystem.Infrastructure.Services
{
    public class CVGenerationService : ICVGenerationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CVGenerationService> _logger;
        private readonly string _outputPath;

        public CVGenerationService(ApplicationDbContext context, ILogger<CVGenerationService> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _outputPath = configuration["CVOutputPath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "GeneratedCVs");
            
            // Ensure output directory exists
            if (!Directory.Exists(_outputPath))
            {
                Directory.CreateDirectory(_outputPath);
            }
        }

        public async Task<CVGenerationResult> GenerateCVAsync(CVGenerationRequest request)
        {
            try
            {
                var userProfile = await _context.UserProfiles
                    .Include(u => u.Skills)
                    .Include(u => u.WorkExperiences)
                    .Include(u => u.Educations)
                    .Include(u => u.Certifications)
                    .Include(u => u.Projects)
                    .FirstOrDefaultAsync(u => u.Id == request.UserProfileId);

                var jobDescription = await _context.JobDescriptions
                    .FirstOrDefaultAsync(j => j.Id == request.JobDescriptionId);

                if (userProfile == null || jobDescription == null)
                {
                    throw new ArgumentException("User profile or job description not found");
                }

                // Sanitize file name to prevent path traversal attacks
                var sanitizedFirstName = SanitizeFileName(userProfile.FirstName);
                var sanitizedLastName = SanitizeFileName(userProfile.LastName);
                var sanitizedJobTitle = SanitizeFileName(jobDescription.JobTitle);
                var fileName = $"{sanitizedFirstName}_{sanitizedLastName}_{sanitizedJobTitle}_{DateTime.Now:yyyyMMdd_HHmmss}";
                var filePath = Path.Combine(_outputPath, $"{fileName}.{request.FileFormat.ToLower()}");

                byte[] fileBytes;
                if (request.FileFormat.ToUpper() == "PDF")
                {
                    fileBytes = await GeneratePDFAsync(request);
                }
                else
                {
                    fileBytes = await GenerateWordAsync(request);
                }

                await File.WriteAllBytesAsync(filePath, fileBytes);

                // Save generated CV record
                var generatedCV = new GeneratedCV
                {
                    FileName = $"{fileName}.{request.FileFormat.ToLower()}",
                    FileFormat = request.FileFormat,
                    TemplateType = request.TemplateType,
                    FilePath = filePath,
                    FileSizeBytes = fileBytes.Length,
                    UserProfileId = request.UserProfileId,
                    JobDescriptionId = request.JobDescriptionId
                };

                _context.GeneratedCVs.Add(generatedCV);
                await _context.SaveChangesAsync();

                return new CVGenerationResult
                {
                    GeneratedCVId = generatedCV.Id,
                    FileName = generatedCV.FileName,
                    FilePath = generatedCV.FilePath,
                    FileFormat = generatedCV.FileFormat,
                    TemplateType = generatedCV.TemplateType,
                    FileSizeBytes = generatedCV.FileSizeBytes,
                    GeneratedAt = generatedCV.GeneratedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating CV for user {UserId}", request.UserProfileId);
                throw;
            }
        }

        public async Task<byte[]> GeneratePDFAsync(CVGenerationRequest request)
        {
            try
            {
                var userProfile = await _context.UserProfiles
                    .Include(u => u.Skills)
                    .Include(u => u.WorkExperiences)
                    .Include(u => u.Educations)
                    .Include(u => u.Certifications)
                    .Include(u => u.Projects)
                    .FirstOrDefaultAsync(u => u.Id == request.UserProfileId);

                if (userProfile == null)
                {
                    throw new ArgumentException("User profile not found");
                }

                var document = QuestPDF.Fluent.Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(12));

                        page.Header()
                            .Text($"{userProfile.FirstName} {userProfile.LastName}")
                            .SemiBold().FontSize(24).FontColor(Colors.Blue.Medium);

                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre)
                            .Column(x =>
                            {
                                x.Spacing(20);

                                // Contact Information
                                x.Item().Text("Contact Information")
                                    .SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);
                                
                                x.Item().Text($"{userProfile.Email}")
                                    .FontSize(10);
                                
                                if (!string.IsNullOrEmpty(userProfile.PhoneNumber))
                                {
                                    x.Item().Text($"{userProfile.PhoneNumber}")
                                        .FontSize(10);
                                }
                                
                                if (!string.IsNullOrEmpty(userProfile.LinkedInProfile))
                                {
                                    x.Item().Text($"LinkedIn: {userProfile.LinkedInProfile}")
                                        .FontSize(10);
                                }

                                // Professional Summary
                                if (!string.IsNullOrEmpty(userProfile.ProfessionalSummary))
                                {
                                    x.Item().Text("Professional Summary")
                                        .SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);
                                    
                                    x.Item().Text(userProfile.ProfessionalSummary)
                                        .FontSize(10);
                                }

                                // Skills
                                if (userProfile.Skills.Any())
                                {
                                    x.Item().Text("Skills")
                                        .SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);
                                    
                                    var highlightedSkills = userProfile.Skills
                                        .Where(s => request.HighlightedSkills.Contains(s.Name))
                                        .ToList();
                                    
                                    var regularSkills = userProfile.Skills
                                        .Where(s => !request.HighlightedSkills.Contains(s.Name))
                                        .ToList();

                                    if (highlightedSkills.Any())
                                    {
                                        x.Item().Text("Key Skills:")
                                            .SemiBold().FontSize(12);
                                        
                                        foreach (var skill in highlightedSkills)
                                        {
                                            x.Item().Text($"• {skill.Name} ({skill.ProficiencyLevel})")
                                                .FontSize(10).FontColor(Colors.Blue.Medium);
                                        }
                                    }

                                    if (regularSkills.Any())
                                    {
                                        x.Item().Text("Additional Skills:")
                                            .SemiBold().FontSize(12);
                                        
                                        var skillText = string.Join(", ", regularSkills.Select(s => s.Name));
                                        x.Item().Text(skillText)
                                            .FontSize(10);
                                    }
                                }

                                // Work Experience
                                if (userProfile.WorkExperiences.Any())
                                {
                                    x.Item().Text("Work Experience")
                                        .SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);
                                    
                                    var highlightedExperiences = userProfile.WorkExperiences
                                        .Where(w => request.HighlightedExperience.Contains(w.CompanyName))
                                        .OrderByDescending(w => w.StartDate)
                                        .ToList();
                                    
                                    var regularExperiences = userProfile.WorkExperiences
                                        .Where(w => !request.HighlightedExperience.Contains(w.CompanyName))
                                        .OrderByDescending(w => w.StartDate)
                                        .ToList();

                                    foreach (var exp in highlightedExperiences.Concat(regularExperiences))
                                    {
                                        x.Item().Text($"{exp.JobTitle} at {exp.CompanyName}")
                                            .SemiBold().FontSize(12);
                                        
                                        x.Item().Text($"{exp.StartDate:MMM yyyy} - {(exp.IsCurrentlyWorking ? "Present" : exp.EndDate?.ToString("MMM yyyy"))}")
                                            .FontSize(10).FontColor(Colors.Grey.Medium);
                                        
                                        if (!string.IsNullOrEmpty(exp.Description))
                                        {
                                            x.Item().Text(exp.Description)
                                                .FontSize(10);
                                        }
                                        
                                        x.Item().PaddingTop(5);
                                    }
                                }

                                // Education
                                if (userProfile.Educations.Any())
                                {
                                    x.Item().Text("Education")
                                        .SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);
                                    
                                    foreach (var edu in userProfile.Educations.OrderByDescending(e => e.StartDate))
                                    {
                                        x.Item().Text($"{edu.Degree} in {edu.FieldOfStudy}")
                                            .SemiBold().FontSize(12);
                                        
                                        x.Item().Text($"{edu.Institution}")
                                            .FontSize(10);
                                        
                                        x.Item().Text($"{edu.StartDate:MMM yyyy} - {(edu.IsCurrentlyStudying ? "Present" : edu.EndDate?.ToString("MMM yyyy"))}")
                                            .FontSize(10).FontColor(Colors.Grey.Medium);
                                        
                                        x.Item().PaddingTop(5);
                                    }
                                }

                                // Certifications
                                if (userProfile.Certifications.Any())
                                {
                                    x.Item().Text("Certifications")
                                        .SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);
                                    
                                    foreach (var cert in userProfile.Certifications.OrderByDescending(c => c.IssueDate))
                                    {
                                        x.Item().Text($"{cert.Name}")
                                            .SemiBold().FontSize(12);
                                        
                                        if (!string.IsNullOrEmpty(cert.IssuingOrganization))
                                        {
                                            x.Item().Text($"{cert.IssuingOrganization}")
                                                .FontSize(10);
                                        }
                                        
                                        x.Item().Text($"Issued: {cert.IssueDate:MMM yyyy}")
                                            .FontSize(10).FontColor(Colors.Grey.Medium);
                                        
                                        x.Item().PaddingTop(5);
                                    }
                                }
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span("Page ");
                                x.CurrentPageNumber();
                            });
                    });
                });

                return document.GeneratePdf();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF CV for user {UserId}", request.UserProfileId);
                throw;
            }
        }

        public async Task<byte[]> GenerateWordAsync(CVGenerationRequest request)
        {
            try
            {
                var userProfile = await _context.UserProfiles
                    .Include(u => u.Skills)
                    .Include(u => u.WorkExperiences)
                    .Include(u => u.Educations)
                    .Include(u => u.Certifications)
                    .Include(u => u.Projects)
                    .FirstOrDefaultAsync(u => u.Id == request.UserProfileId);

                if (userProfile == null)
                {
                    throw new ArgumentException("User profile not found");
                }

                using var document = DocX.Create(new MemoryStream());
                
                // Title
                var title = document.InsertParagraph($"{userProfile.FirstName} {userProfile.LastName}");
                title.Alignment = Alignment.center;
                title.FontSize(24);
                title.Color(System.Drawing.Color.Blue);
                title.Bold();

                // Contact Information
                var contact = document.InsertParagraph();
                contact.AppendLine($"Email: {userProfile.Email}");
                if (!string.IsNullOrEmpty(userProfile.PhoneNumber))
                {
                    contact.AppendLine($"Phone: {userProfile.PhoneNumber}");
                }
                if (!string.IsNullOrEmpty(userProfile.LinkedInProfile))
                {
                    contact.AppendLine($"LinkedIn: {userProfile.LinkedInProfile}");
                }
                contact.Alignment = Alignment.center;

                // Professional Summary
                if (!string.IsNullOrEmpty(userProfile.ProfessionalSummary))
                {
                    var summaryTitle = document.InsertParagraph("Professional Summary");
                    summaryTitle.FontSize(16);
                    summaryTitle.Color(System.Drawing.Color.Blue);
                    summaryTitle.Bold();
                    
                    var summary = document.InsertParagraph(userProfile.ProfessionalSummary);
                    summary.FontSize(11);
                }

                // Skills
                if (userProfile.Skills.Any())
                {
                    var skillsTitle = document.InsertParagraph("Skills");
                    skillsTitle.FontSize(16);
                    skillsTitle.Color(System.Drawing.Color.Blue);
                    skillsTitle.Bold();

                    var highlightedSkills = userProfile.Skills
                        .Where(s => request.HighlightedSkills.Contains(s.Name))
                        .ToList();
                    
                    var regularSkills = userProfile.Skills
                        .Where(s => !request.HighlightedSkills.Contains(s.Name))
                        .ToList();

                    if (highlightedSkills.Any())
                    {
                        var keySkills = document.InsertParagraph("Key Skills:");
                        keySkills.Bold();
                        
                        foreach (var skill in highlightedSkills)
                        {
                            var skillPara = document.InsertParagraph($"• {skill.Name} ({skill.ProficiencyLevel})");
                            skillPara.Color(System.Drawing.Color.Blue);
                        }
                    }

                    if (regularSkills.Any())
                    {
                        var additionalSkills = document.InsertParagraph("Additional Skills:");
                        additionalSkills.Bold();
                        
                        var skillText = string.Join(", ", regularSkills.Select(s => s.Name));
                        var skillsPara = document.InsertParagraph(skillText);
                    }
                }

                // Work Experience
                if (userProfile.WorkExperiences.Any())
                {
                    var expTitle = document.InsertParagraph("Work Experience");
                    expTitle.FontSize(16);
                    expTitle.Color(System.Drawing.Color.Blue);
                    expTitle.Bold();

                    var highlightedExperiences = userProfile.WorkExperiences
                        .Where(w => request.HighlightedExperience.Contains(w.CompanyName))
                        .OrderByDescending(w => w.StartDate)
                        .ToList();
                    
                    var regularExperiences = userProfile.WorkExperiences
                        .Where(w => !request.HighlightedExperience.Contains(w.CompanyName))
                        .OrderByDescending(w => w.StartDate)
                        .ToList();

                    foreach (var exp in highlightedExperiences.Concat(regularExperiences))
                    {
                        var jobTitle = document.InsertParagraph($"{exp.JobTitle} at {exp.CompanyName}");
                        jobTitle.Bold();
                        
                        var dateRange = document.InsertParagraph($"{exp.StartDate:MMM yyyy} - {(exp.IsCurrentlyWorking ? "Present" : exp.EndDate?.ToString("MMM yyyy"))}");
                        dateRange.Color(System.Drawing.Color.Gray);
                        
                        if (!string.IsNullOrEmpty(exp.Description))
                        {
                            var description = document.InsertParagraph(exp.Description);
                            description.FontSize(11);
                        }
                    }
                }

                // Education
                if (userProfile.Educations.Any())
                {
                    var eduTitle = document.InsertParagraph("Education");
                    eduTitle.FontSize(16);
                    eduTitle.Color(System.Drawing.Color.Blue);
                    eduTitle.Bold();

                    foreach (var edu in userProfile.Educations.OrderByDescending(e => e.StartDate))
                    {
                        var degree = document.InsertParagraph($"{edu.Degree} in {edu.FieldOfStudy}");
                        degree.Bold();
                        
                        var institution = document.InsertParagraph(edu.Institution);
                        
                        var dateRange = document.InsertParagraph($"{edu.StartDate:MMM yyyy} - {(edu.IsCurrentlyStudying ? "Present" : edu.EndDate?.ToString("MMM yyyy"))}");
                        dateRange.Color(System.Drawing.Color.Gray);
                    }
                }

                // Certifications
                if (userProfile.Certifications.Any())
                {
                    var certTitle = document.InsertParagraph("Certifications");
                    certTitle.FontSize(16);
                    certTitle.Color(System.Drawing.Color.Blue);
                    certTitle.Bold();

                    foreach (var cert in userProfile.Certifications.OrderByDescending(c => c.IssueDate))
                    {
                        var certName = document.InsertParagraph(cert.Name);
                        certName.Bold();
                        
                        if (!string.IsNullOrEmpty(cert.IssuingOrganization))
                        {
                            var org = document.InsertParagraph(cert.IssuingOrganization);
                        }
                        
                        var issueDate = document.InsertParagraph($"Issued: {cert.IssueDate:MMM yyyy}");
                        issueDate.Color(System.Drawing.Color.Gray);
                    }
                }

                using var stream = new MemoryStream();
                document.SaveAs(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Word CV for user {UserId}", request.UserProfileId);
                throw;
            }
        }

        public async Task<List<CVTemplate>> GetAvailableTemplatesAsync()
        {
            return new List<CVTemplate>
            {
                new CVTemplate
                {
                    Name = "Professional",
                    Description = "Clean and professional layout suitable for corporate positions",
                    Type = "Professional",
                    IsATSCompatible = true,
                    SupportedFormats = new List<string> { "PDF", "DOCX" }
                },
                new CVTemplate
                {
                    Name = "Modern",
                    Description = "Modern design with visual elements for creative positions",
                    Type = "Modern",
                    IsATSCompatible = false,
                    SupportedFormats = new List<string> { "PDF" }
                },
                new CVTemplate
                {
                    Name = "ATS-Friendly",
                    Description = "Optimized for Applicant Tracking Systems",
                    Type = "ATS-Friendly",
                    IsATSCompatible = true,
                    SupportedFormats = new List<string> { "PDF", "DOCX" }
                }
            };
        }

        public async Task<CVGenerationResult?> GetGeneratedCVAsync(int generatedCVId)
        {
            var generatedCV = await _context.GeneratedCVs
                .FirstOrDefaultAsync(cv => cv.Id == generatedCVId);

            if (generatedCV == null)
                return null;

            return new CVGenerationResult
            {
                GeneratedCVId = generatedCV.Id,
                FileName = generatedCV.FileName,
                FilePath = generatedCV.FilePath,
                FileFormat = generatedCV.FileFormat,
                TemplateType = generatedCV.TemplateType,
                FileSizeBytes = generatedCV.FileSizeBytes,
                GeneratedAt = generatedCV.GeneratedAt
            };
        }

        public async Task<List<CVGenerationResult>> GetGeneratedCVsForUserAsync(int userProfileId)
        {
            var generatedCVs = await _context.GeneratedCVs
                .Where(cv => cv.UserProfileId == userProfileId)
                .OrderByDescending(cv => cv.GeneratedAt)
                .ToListAsync();

            return generatedCVs.Select(cv => new CVGenerationResult
            {
                GeneratedCVId = cv.Id,
                FileName = cv.FileName,
                FilePath = cv.FilePath,
                FileFormat = cv.FileFormat,
                TemplateType = cv.TemplateType,
                FileSizeBytes = cv.FileSizeBytes,
                GeneratedAt = cv.GeneratedAt
            }).ToList();
        }

        public async Task<bool> DeleteGeneratedCVAsync(int generatedCVId)
        {
            try
            {
                var generatedCV = await _context.GeneratedCVs
                    .FirstOrDefaultAsync(cv => cv.Id == generatedCVId);

                if (generatedCV == null)
                    return false;

                // Delete the physical file
                if (File.Exists(generatedCV.FilePath))
                {
                    File.Delete(generatedCV.FilePath);
                }

                // Remove from database
                _context.GeneratedCVs.Remove(generatedCV);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting generated CV {CVId}", generatedCVId);
                return false;
            }
        }

        private static string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "Unknown";

            // Remove or replace invalid characters
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
            
            // Limit length and remove any remaining dangerous characters
            sanitized = sanitized.Replace("..", "_").Replace("/", "_").Replace("\\", "_");
            
            return sanitized.Length > 50 ? sanitized.Substring(0, 50) : sanitized;
        }
    }
}
