using AutomatedCVSystem.Core.DTOs;
using AutomatedCVSystem.Core.Interfaces;
using AutomatedCVSystem.Core.Models;
using AutomatedCVSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AutomatedCVSystem.Infrastructure.Services
{
    public class JobAnalysisService : IJobAnalysisService
    {
        private readonly ApplicationDbContext _context;
        private readonly MLContext _mlContext;
        private readonly ILogger<JobAnalysisService> _logger;

        // Common skill keywords for extraction
        private readonly HashSet<string> _technicalSkills = new(StringComparer.OrdinalIgnoreCase)
        {
            "C#", "C++", "Java", "Python", "JavaScript", "TypeScript", "React", "Angular", "Vue.js",
            "Node.js", "ASP.NET", ".NET", "SQL", "MySQL", "PostgreSQL", "MongoDB", "Azure", "AWS",
            "Docker", "Kubernetes", "Git", "GitHub", "GitLab", "Jenkins", "CI/CD", "REST API",
            "GraphQL", "Microservices", "Agile", "Scrum", "DevOps", "Machine Learning", "AI",
            "Data Science", "TensorFlow", "PyTorch", "Pandas", "NumPy", "HTML", "CSS", "Bootstrap",
            "jQuery", "Webpack", "Babel", "ES6", "JSON", "XML", "Linux", "Windows", "macOS"
        };

        private readonly HashSet<string> _softSkills = new(StringComparer.OrdinalIgnoreCase)
        {
            "Communication", "Leadership", "Teamwork", "Problem Solving", "Critical Thinking",
            "Time Management", "Adaptability", "Creativity", "Analytical", "Detail Oriented",
            "Project Management", "Customer Service", "Presentation", "Negotiation", "Mentoring"
        };

        public JobAnalysisService(ApplicationDbContext context, ILogger<JobAnalysisService> logger)
        {
            _context = context;
            _mlContext = new MLContext();
            _logger = logger;
        }

        public async Task<JobAnalysisResult> AnalyzeJobDescriptionAsync(int userProfileId, JobAnalysisRequest request)
        {
            try
            {
                // Extract information from job description
                var extractedSkills = await ExtractSkillsAsync(request.JobDescription);
                var extractedRequirements = await ExtractRequirementsAsync(request.JobDescription);
                var extractedExperience = await ExtractExperienceRequirementsAsync(request.JobDescription);
                var extractedEducation = await ExtractEducationRequirementsAsync(request.JobDescription);

                // Save job description to database
                var jobDescription = new JobDescription
                {
                    JobTitle = request.JobTitle,
                    CompanyName = request.CompanyName,
                    Location = request.Location,
                    FullDescription = request.JobDescription,
                    ExtractedSkills = JsonSerializer.Serialize(extractedSkills),
                    ExtractedRequirements = JsonSerializer.Serialize(extractedRequirements),
                    ExtractedExperience = JsonSerializer.Serialize(extractedExperience),
                    ExtractedEducation = JsonSerializer.Serialize(extractedEducation),
                    UserProfileId = userProfileId
                };

                _context.JobDescriptions.Add(jobDescription);
                await _context.SaveChangesAsync();

                // Calculate matching score
                var matchingScore = await CalculateMatchingScoreAsync(userProfileId, request);

                return new JobAnalysisResult
                {
                    JobDescriptionId = jobDescription.Id,
                    JobTitle = request.JobTitle,
                    CompanyName = request.CompanyName,
                    Location = request.Location,
                    ExtractedSkills = extractedSkills,
                    ExtractedRequirements = extractedRequirements,
                    ExtractedExperience = extractedExperience,
                    ExtractedEducation = extractedEducation,
                    MatchingScore = matchingScore
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing job description for user {UserId}", userProfileId);
                throw;
            }
        }

        public async Task<List<string>> ExtractSkillsAsync(string jobDescription)
        {
            var skills = new List<string>();
            var text = jobDescription.ToLowerInvariant();

            // Extract technical skills
            foreach (var skill in _technicalSkills)
            {
                if (text.Contains(skill.ToLowerInvariant()))
                {
                    skills.Add(skill);
                }
            }

            // Extract soft skills
            foreach (var skill in _softSkills)
            {
                if (text.Contains(skill.ToLowerInvariant()))
                {
                    skills.Add(skill);
                }
            }

            // Use regex to find additional skills mentioned in the text
            var skillPatterns = new[]
            {
                @"\b(?:experience with|knowledge of|proficient in|skilled in)\s+([^.,\n]+)",
                @"\b(?:required|preferred|must have|should have)\s*:?\s*([^.,\n]+)",
                @"\b(?:technologies?|tools?|frameworks?|languages?)\s*:?\s*([^.,\n]+)"
            };

            foreach (var pattern in skillPatterns)
            {
                var matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    var skill = match.Groups[1].Value.Trim();
                    if (!string.IsNullOrEmpty(skill) && skill.Length > 2 && skill.Length < 50)
                    {
                        skills.Add(skill);
                    }
                }
            }

            return skills.Distinct().ToList();
        }

        public async Task<List<string>> ExtractRequirementsAsync(string jobDescription)
        {
            var requirements = new List<string>();
            var text = jobDescription.ToLowerInvariant();

            // Common requirement patterns
            var requirementPatterns = new[]
            {
                @"\b(?:bachelor|master|phd|degree)\s+(?:in|of)?\s*([^.,\n]+)",
                @"\b(?:years?|yrs?)\s+(?:of\s+)?(?:experience|exp)\s+(?:in|with)?\s*([^.,\n]+)",
                @"\b(?:minimum|at least|required)\s+(\d+)\s+(?:years?|yrs?)\s+(?:of\s+)?(?:experience|exp)",
                @"\b(?:strong|excellent|good)\s+(?:knowledge|understanding|experience)\s+(?:of|in)\s*([^.,\n]+)",
                @"\b(?:ability|capability)\s+(?:to|of)\s*([^.,\n]+)",
                @"\b(?:familiarity|proficiency)\s+(?:with|in)\s*([^.,\n]+)"
            };

            foreach (var pattern in requirementPatterns)
            {
                var matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    var requirement = match.Groups[1].Value.Trim();
                    if (!string.IsNullOrEmpty(requirement) && requirement.Length > 3 && requirement.Length < 100)
                    {
                        requirements.Add(requirement);
                    }
                }
            }

            return requirements.Distinct().ToList();
        }

        public async Task<List<string>> ExtractExperienceRequirementsAsync(string jobDescription)
        {
            var experience = new List<string>();
            var text = jobDescription.ToLowerInvariant();

            // Experience patterns
            var experiencePatterns = new[]
            {
                @"\b(\d+)\s*(?:to\s*)?(\d+)?\s*(?:years?|yrs?)\s+(?:of\s+)?(?:experience|exp)\s+(?:in|with)?\s*([^.,\n]+)",
                @"\b(?:minimum|at least|required)\s+(\d+)\s+(?:years?|yrs?)\s+(?:of\s+)?(?:experience|exp)",
                @"\b(?:senior|junior|mid-level|entry-level|experienced)\s+([^.,\n]+)",
                @"\b(?:lead|principal|architect|manager|director)\s+([^.,\n]+)"
            };

            foreach (var pattern in experiencePatterns)
            {
                var matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    var exp = match.Groups[1].Value.Trim();
                    if (!string.IsNullOrEmpty(exp))
                    {
                        experience.Add(exp);
                    }
                }
            }

            return experience.Distinct().ToList();
        }

        public async Task<List<string>> ExtractEducationRequirementsAsync(string jobDescription)
        {
            var education = new List<string>();
            var text = jobDescription.ToLowerInvariant();

            // Education patterns
            var educationPatterns = new[]
            {
                @"\b(?:bachelor|master|phd|doctorate|associate)\s+(?:degree|of|in)?\s*([^.,\n]+)",
                @"\b(?:computer science|engineering|mathematics|business|economics|statistics)\s+(?:degree|major|background)",
                @"\b(?:university|college|institute)\s+(?:degree|education|background)",
                @"\b(?:certification|certified)\s+(?:in|for)?\s*([^.,\n]+)"
            };

            foreach (var pattern in educationPatterns)
            {
                var matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    var edu = match.Groups[1].Value.Trim();
                    if (!string.IsNullOrEmpty(edu))
                    {
                        education.Add(edu);
                    }
                }
            }

            return education.Distinct().ToList();
        }

        public async Task<MatchingScore> CalculateMatchingScoreAsync(int userProfileId, JobAnalysisRequest request)
        {
            try
            {
                var userProfile = await _context.UserProfiles
                    .Include(u => u.Skills)
                    .Include(u => u.WorkExperiences)
                    .Include(u => u.Educations)
                    .FirstOrDefaultAsync(u => u.Id == userProfileId);

                if (userProfile == null)
                {
                    throw new ArgumentException($"User profile with ID {userProfileId} not found");
                }

                var extractedSkills = await ExtractSkillsAsync(request.JobDescription);
                var extractedExperience = await ExtractExperienceRequirementsAsync(request.JobDescription);
                var extractedEducation = await ExtractEducationRequirementsAsync(request.JobDescription);

                // Calculate skill matching
                var skillMatches = CalculateSkillMatches(userProfile.Skills.ToList(), extractedSkills);
                var skillsMatch = skillMatches.Any() ? skillMatches.Average(s => s.MatchScore) : 0;

                // Calculate experience matching
                var experienceMatches = CalculateExperienceMatches(userProfile.WorkExperiences.ToList(), extractedExperience);
                var experienceMatch = experienceMatches.Any() ? experienceMatches.Average(e => e.MatchScore) : 0;

                // Calculate education matching
                var educationMatches = CalculateEducationMatches(userProfile.Educations.ToList(), extractedEducation);
                var educationMatch = educationMatches.Any() ? educationMatches.Average(e => e.MatchScore) : 0;

                // Calculate overall score (weighted average)
                var overallScore = (skillsMatch * 0.4) + (experienceMatch * 0.4) + (educationMatch * 0.2);

                return new MatchingScore
                {
                    OverallScore = Math.Round(overallScore * 100, 2),
                    SkillsMatch = Math.Round(skillsMatch * 100, 2),
                    ExperienceMatch = Math.Round(experienceMatch * 100, 2),
                    EducationMatch = Math.Round(educationMatch * 100, 2),
                    SkillMatches = skillMatches,
                    ExperienceMatches = experienceMatches,
                    EducationMatches = educationMatches
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating matching score for user {UserId}", userProfileId);
                throw;
            }
        }

        private List<SkillMatch> CalculateSkillMatches(List<Skill> userSkills, List<string> jobSkills)
        {
            var matches = new List<SkillMatch>();

            foreach (var userSkill in userSkills)
            {
                var bestMatch = 0.0;
                var isExactMatch = false;

                foreach (var jobSkill in jobSkills)
                {
                    var similarity = CalculateStringSimilarity(userSkill.Name, jobSkill);
                    if (similarity > bestMatch)
                    {
                        bestMatch = similarity;
                        isExactMatch = string.Equals(userSkill.Name, jobSkill, StringComparison.OrdinalIgnoreCase);
                    }
                }

                if (bestMatch > 0.3) // Threshold for considering a match
                {
                    matches.Add(new SkillMatch
                    {
                        SkillName = userSkill.Name,
                        MatchScore = bestMatch,
                        IsExactMatch = isExactMatch,
                        IsHighlighted = bestMatch > 0.7
                    });
                }
            }

            return matches.OrderByDescending(m => m.MatchScore).ToList();
        }

        private List<ExperienceMatch> CalculateExperienceMatches(List<WorkExperience> userExperiences, List<string> jobExperience)
        {
            var matches = new List<ExperienceMatch>();

            foreach (var userExp in userExperiences)
            {
                var bestMatch = 0.0;

                foreach (var jobExp in jobExperience)
                {
                    var companyMatch = CalculateStringSimilarity(userExp.CompanyName, jobExp);
                    var titleMatch = CalculateStringSimilarity(userExp.JobTitle, jobExp);
                    var best = Math.Max(companyMatch, titleMatch);
                    
                    if (best > bestMatch)
                    {
                        bestMatch = best;
                    }
                }

                if (bestMatch > 0.3)
                {
                    matches.Add(new ExperienceMatch
                    {
                        CompanyName = userExp.CompanyName,
                        JobTitle = userExp.JobTitle,
                        MatchScore = bestMatch,
                        IsHighlighted = bestMatch > 0.7
                    });
                }
            }

            return matches.OrderByDescending(m => m.MatchScore).ToList();
        }

        private List<EducationMatch> CalculateEducationMatches(List<Education> userEducations, List<string> jobEducation)
        {
            var matches = new List<EducationMatch>();

            foreach (var userEdu in userEducations)
            {
                var bestMatch = 0.0;

                foreach (var jobEdu in jobEducation)
                {
                    var institutionMatch = CalculateStringSimilarity(userEdu.Institution, jobEdu);
                    var degreeMatch = CalculateStringSimilarity(userEdu.Degree, jobEdu);
                    var fieldMatch = CalculateStringSimilarity(userEdu.FieldOfStudy ?? "", jobEdu);
                    var best = Math.Max(Math.Max(institutionMatch, degreeMatch), fieldMatch);
                    
                    if (best > bestMatch)
                    {
                        bestMatch = best;
                    }
                }

                if (bestMatch > 0.3)
                {
                    matches.Add(new EducationMatch
                    {
                        Institution = userEdu.Institution,
                        Degree = userEdu.Degree,
                        MatchScore = bestMatch,
                        IsHighlighted = bestMatch > 0.7
                    });
                }
            }

            return matches.OrderByDescending(m => m.MatchScore).ToList();
        }

        private double CalculateStringSimilarity(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
                return 0;

            str1 = str1.ToLowerInvariant().Trim();
            str2 = str2.ToLowerInvariant().Trim();

            if (str1 == str2)
                return 1.0;

            // Simple Levenshtein distance-based similarity
            var distance = LevenshteinDistance(str1, str2);
            var maxLength = Math.Max(str1.Length, str2.Length);
            
            return maxLength == 0 ? 0 : 1.0 - (double)distance / maxLength;
        }

        private int LevenshteinDistance(string str1, string str2)
        {
            var matrix = new int[str1.Length + 1, str2.Length + 1];

            for (int i = 0; i <= str1.Length; i++)
                matrix[i, 0] = i;

            for (int j = 0; j <= str2.Length; j++)
                matrix[0, j] = j;

            for (int i = 1; i <= str1.Length; i++)
            {
                for (int j = 1; j <= str2.Length; j++)
                {
                    var cost = str1[i - 1] == str2[j - 1] ? 0 : 1;
                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }

            return matrix[str1.Length, str2.Length];
        }
    }
}
