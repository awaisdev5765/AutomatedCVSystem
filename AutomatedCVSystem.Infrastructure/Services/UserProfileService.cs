using AutomatedCVSystem.Core.DTOs;
using AutomatedCVSystem.Core.Interfaces;
using AutomatedCVSystem.Core.Models;
using AutomatedCVSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AutomatedCVSystem.Infrastructure.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserProfileService> _logger;

        public UserProfileService(ApplicationDbContext context, ILogger<UserProfileService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(int id)
        {
            try
            {
                var userProfile = await _context.UserProfiles
                    .Include(u => u.Educations)
                    .Include(u => u.WorkExperiences)
                    .Include(u => u.Skills)
                    .Include(u => u.Certifications)
                    .Include(u => u.Projects)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (userProfile == null)
                    return null;

                return MapToDto(userProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile {UserId}", id);
                throw;
            }
        }

        public async Task<UserProfileDto> CreateUserProfileAsync(UserProfileDto userProfileDto)
        {
            try
            {
                var userProfile = new UserProfile
                {
                    FirstName = userProfileDto.FirstName,
                    LastName = userProfileDto.LastName,
                    Email = userProfileDto.Email,
                    PhoneNumber = userProfileDto.PhoneNumber,
                    Address = userProfileDto.Address,
                    City = userProfileDto.City,
                    Country = userProfileDto.Country,
                    PostalCode = userProfileDto.PostalCode,
                    ProfessionalSummary = userProfileDto.ProfessionalSummary,
                    LinkedInProfile = userProfileDto.LinkedInProfile,
                    GitHubProfile = userProfileDto.GitHubProfile,
                    PortfolioWebsite = userProfileDto.PortfolioWebsite,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.UserProfiles.Add(userProfile);
                await _context.SaveChangesAsync();

                return MapToDto(userProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user profile");
                throw;
            }
        }

        public async Task<UserProfileDto> UpdateUserProfileAsync(int id, UserProfileDto userProfileDto)
        {
            try
            {
                var userProfile = await _context.UserProfiles.FindAsync(id);
                if (userProfile == null)
                    throw new ArgumentException($"User profile with ID {id} not found");

                userProfile.FirstName = userProfileDto.FirstName;
                userProfile.LastName = userProfileDto.LastName;
                userProfile.Email = userProfileDto.Email;
                userProfile.PhoneNumber = userProfileDto.PhoneNumber;
                userProfile.Address = userProfileDto.Address;
                userProfile.City = userProfileDto.City;
                userProfile.Country = userProfileDto.Country;
                userProfile.PostalCode = userProfileDto.PostalCode;
                userProfile.ProfessionalSummary = userProfileDto.ProfessionalSummary;
                userProfile.LinkedInProfile = userProfileDto.LinkedInProfile;
                userProfile.GitHubProfile = userProfileDto.GitHubProfile;
                userProfile.PortfolioWebsite = userProfileDto.PortfolioWebsite;
                userProfile.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return await GetUserProfileAsync(id) ?? throw new InvalidOperationException("Failed to retrieve updated user profile");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user profile {UserId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteUserProfileAsync(int id)
        {
            try
            {
                var userProfile = await _context.UserProfiles.FindAsync(id);
                if (userProfile == null)
                    return false;

                _context.UserProfiles.Remove(userProfile);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user profile {UserId}", id);
                return false;
            }
        }

        public async Task<List<UserProfileDto>> GetAllUserProfilesAsync()
        {
            try
            {
                var userProfiles = await _context.UserProfiles
                    .Include(u => u.Educations)
                    .Include(u => u.WorkExperiences)
                    .Include(u => u.Skills)
                    .Include(u => u.Certifications)
                    .Include(u => u.Projects)
                    .ToListAsync();

                return userProfiles.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all user profiles");
                throw;
            }
        }

        public async Task<EducationDto> AddEducationAsync(int userProfileId, EducationDto educationDto)
        {
            try
            {
                var education = new Education
                {
                    Institution = educationDto.Institution,
                    Degree = educationDto.Degree,
                    FieldOfStudy = educationDto.FieldOfStudy,
                    Grade = educationDto.Grade,
                    StartDate = educationDto.StartDate,
                    EndDate = educationDto.EndDate,
                    IsCurrentlyStudying = educationDto.IsCurrentlyStudying,
                    Description = educationDto.Description,
                    Location = educationDto.Location,
                    UserProfileId = userProfileId
                };

                _context.Educations.Add(education);
                await _context.SaveChangesAsync();

                return MapToDto(education);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding education for user {UserId}", userProfileId);
                throw;
            }
        }

        public async Task<EducationDto> UpdateEducationAsync(int userProfileId, int educationId, EducationDto educationDto)
        {
            try
            {
                var education = await _context.Educations
                    .FirstOrDefaultAsync(e => e.Id == educationId && e.UserProfileId == userProfileId);

                if (education == null)
                    throw new ArgumentException("Education not found");

                education.Institution = educationDto.Institution;
                education.Degree = educationDto.Degree;
                education.FieldOfStudy = educationDto.FieldOfStudy;
                education.Grade = educationDto.Grade;
                education.StartDate = educationDto.StartDate;
                education.EndDate = educationDto.EndDate;
                education.IsCurrentlyStudying = educationDto.IsCurrentlyStudying;
                education.Description = educationDto.Description;
                education.Location = educationDto.Location;

                await _context.SaveChangesAsync();

                return MapToDto(education);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating education {EducationId} for user {UserId}", educationId, userProfileId);
                throw;
            }
        }

        public async Task<bool> DeleteEducationAsync(int userProfileId, int educationId)
        {
            try
            {
                var education = await _context.Educations
                    .FirstOrDefaultAsync(e => e.Id == educationId && e.UserProfileId == userProfileId);

                if (education == null)
                    return false;

                _context.Educations.Remove(education);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting education {EducationId} for user {UserId}", educationId, userProfileId);
                return false;
            }
        }

        public async Task<WorkExperienceDto> AddWorkExperienceAsync(int userProfileId, WorkExperienceDto workExperienceDto)
        {
            try
            {
                var workExperience = new WorkExperience
                {
                    CompanyName = workExperienceDto.CompanyName,
                    JobTitle = workExperienceDto.JobTitle,
                    Location = workExperienceDto.Location,
                    StartDate = workExperienceDto.StartDate,
                    EndDate = workExperienceDto.EndDate,
                    IsCurrentlyWorking = workExperienceDto.IsCurrentlyWorking,
                    Description = workExperienceDto.Description,
                    Achievements = workExperienceDto.Achievements,
                    Department = workExperienceDto.Department,
                    EmploymentType = workExperienceDto.EmploymentType,
                    UserProfileId = userProfileId
                };

                _context.WorkExperiences.Add(workExperience);
                await _context.SaveChangesAsync();

                return MapToDto(workExperience);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding work experience for user {UserId}", userProfileId);
                throw;
            }
        }

        public async Task<WorkExperienceDto> UpdateWorkExperienceAsync(int userProfileId, int workExperienceId, WorkExperienceDto workExperienceDto)
        {
            try
            {
                var workExperience = await _context.WorkExperiences
                    .FirstOrDefaultAsync(w => w.Id == workExperienceId && w.UserProfileId == userProfileId);

                if (workExperience == null)
                    throw new ArgumentException("Work experience not found");

                workExperience.CompanyName = workExperienceDto.CompanyName;
                workExperience.JobTitle = workExperienceDto.JobTitle;
                workExperience.Location = workExperienceDto.Location;
                workExperience.StartDate = workExperienceDto.StartDate;
                workExperience.EndDate = workExperienceDto.EndDate;
                workExperience.IsCurrentlyWorking = workExperienceDto.IsCurrentlyWorking;
                workExperience.Description = workExperienceDto.Description;
                workExperience.Achievements = workExperienceDto.Achievements;
                workExperience.Department = workExperienceDto.Department;
                workExperience.EmploymentType = workExperienceDto.EmploymentType;

                await _context.SaveChangesAsync();

                return MapToDto(workExperience);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating work experience {WorkExperienceId} for user {UserId}", workExperienceId, userProfileId);
                throw;
            }
        }

        public async Task<bool> DeleteWorkExperienceAsync(int userProfileId, int workExperienceId)
        {
            try
            {
                var workExperience = await _context.WorkExperiences
                    .FirstOrDefaultAsync(w => w.Id == workExperienceId && w.UserProfileId == userProfileId);

                if (workExperience == null)
                    return false;

                _context.WorkExperiences.Remove(workExperience);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting work experience {WorkExperienceId} for user {UserId}", workExperienceId, userProfileId);
                return false;
            }
        }

        public async Task<SkillDto> AddSkillAsync(int userProfileId, SkillDto skillDto)
        {
            try
            {
                var skill = new Skill
                {
                    Name = skillDto.Name,
                    Category = skillDto.Category,
                    ProficiencyLevel = skillDto.ProficiencyLevel,
                    YearsOfExperience = skillDto.YearsOfExperience,
                    Description = skillDto.Description,
                    IsHighlighted = skillDto.IsHighlighted,
                    UserProfileId = userProfileId
                };

                _context.Skills.Add(skill);
                await _context.SaveChangesAsync();

                return MapToDto(skill);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding skill for user {UserId}", userProfileId);
                throw;
            }
        }

        public async Task<SkillDto> UpdateSkillAsync(int userProfileId, int skillId, SkillDto skillDto)
        {
            try
            {
                var skill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Id == skillId && s.UserProfileId == userProfileId);

                if (skill == null)
                    throw new ArgumentException("Skill not found");

                skill.Name = skillDto.Name;
                skill.Category = skillDto.Category;
                skill.ProficiencyLevel = skillDto.ProficiencyLevel;
                skill.YearsOfExperience = skillDto.YearsOfExperience;
                skill.Description = skillDto.Description;
                skill.IsHighlighted = skillDto.IsHighlighted;

                await _context.SaveChangesAsync();

                return MapToDto(skill);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating skill {SkillId} for user {UserId}", skillId, userProfileId);
                throw;
            }
        }

        public async Task<bool> DeleteSkillAsync(int userProfileId, int skillId)
        {
            try
            {
                var skill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Id == skillId && s.UserProfileId == userProfileId);

                if (skill == null)
                    return false;

                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting skill {SkillId} for user {UserId}", skillId, userProfileId);
                return false;
            }
        }

        public async Task<CertificationDto> AddCertificationAsync(int userProfileId, CertificationDto certificationDto)
        {
            try
            {
                var certification = new Certification
                {
                    Name = certificationDto.Name,
                    IssuingOrganization = certificationDto.IssuingOrganization,
                    IssueDate = certificationDto.IssueDate,
                    ExpirationDate = certificationDto.ExpirationDate,
                    CredentialId = certificationDto.CredentialId,
                    CredentialUrl = certificationDto.CredentialUrl,
                    Description = certificationDto.Description,
                    IsCurrentlyValid = certificationDto.IsCurrentlyValid,
                    UserProfileId = userProfileId
                };

                _context.Certifications.Add(certification);
                await _context.SaveChangesAsync();

                return MapToDto(certification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding certification for user {UserId}", userProfileId);
                throw;
            }
        }

        public async Task<CertificationDto> UpdateCertificationAsync(int userProfileId, int certificationId, CertificationDto certificationDto)
        {
            try
            {
                var certification = await _context.Certifications
                    .FirstOrDefaultAsync(c => c.Id == certificationId && c.UserProfileId == userProfileId);

                if (certification == null)
                    throw new ArgumentException("Certification not found");

                certification.Name = certificationDto.Name;
                certification.IssuingOrganization = certificationDto.IssuingOrganization;
                certification.IssueDate = certificationDto.IssueDate;
                certification.ExpirationDate = certificationDto.ExpirationDate;
                certification.CredentialId = certificationDto.CredentialId;
                certification.CredentialUrl = certificationDto.CredentialUrl;
                certification.Description = certificationDto.Description;
                certification.IsCurrentlyValid = certificationDto.IsCurrentlyValid;

                await _context.SaveChangesAsync();

                return MapToDto(certification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating certification {CertificationId} for user {UserId}", certificationId, userProfileId);
                throw;
            }
        }

        public async Task<bool> DeleteCertificationAsync(int userProfileId, int certificationId)
        {
            try
            {
                var certification = await _context.Certifications
                    .FirstOrDefaultAsync(c => c.Id == certificationId && c.UserProfileId == userProfileId);

                if (certification == null)
                    return false;

                _context.Certifications.Remove(certification);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting certification {CertificationId} for user {UserId}", certificationId, userProfileId);
                return false;
            }
        }

        public async Task<ProjectDto> AddProjectAsync(int userProfileId, ProjectDto projectDto)
        {
            try
            {
                var project = new Project
                {
                    Name = projectDto.Name,
                    Description = projectDto.Description,
                    Technologies = projectDto.Technologies,
                    ProjectUrl = projectDto.ProjectUrl,
                    GitHubUrl = projectDto.GitHubUrl,
                    StartDate = projectDto.StartDate,
                    EndDate = projectDto.EndDate,
                    IsCurrentlyWorking = projectDto.IsCurrentlyWorking,
                    Role = projectDto.Role,
                    Achievements = projectDto.Achievements,
                    UserProfileId = userProfileId
                };

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                return MapToDto(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding project for user {UserId}", userProfileId);
                throw;
            }
        }

        public async Task<ProjectDto> UpdateProjectAsync(int userProfileId, int projectId, ProjectDto projectDto)
        {
            try
            {
                var project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == projectId && p.UserProfileId == userProfileId);

                if (project == null)
                    throw new ArgumentException("Project not found");

                project.Name = projectDto.Name;
                project.Description = projectDto.Description;
                project.Technologies = projectDto.Technologies;
                project.ProjectUrl = projectDto.ProjectUrl;
                project.GitHubUrl = projectDto.GitHubUrl;
                project.StartDate = projectDto.StartDate;
                project.EndDate = projectDto.EndDate;
                project.IsCurrentlyWorking = projectDto.IsCurrentlyWorking;
                project.Role = projectDto.Role;
                project.Achievements = projectDto.Achievements;

                await _context.SaveChangesAsync();

                return MapToDto(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project {ProjectId} for user {UserId}", projectId, userProfileId);
                throw;
            }
        }

        public async Task<bool> DeleteProjectAsync(int userProfileId, int projectId)
        {
            try
            {
                var project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == projectId && p.UserProfileId == userProfileId);

                if (project == null)
                    return false;

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project {ProjectId} for user {UserId}", projectId, userProfileId);
                return false;
            }
        }

        private static UserProfileDto MapToDto(UserProfile userProfile)
        {
            return new UserProfileDto
            {
                Id = userProfile.Id,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Email = userProfile.Email,
                PhoneNumber = userProfile.PhoneNumber,
                Address = userProfile.Address,
                City = userProfile.City,
                Country = userProfile.Country,
                PostalCode = userProfile.PostalCode,
                ProfessionalSummary = userProfile.ProfessionalSummary,
                LinkedInProfile = userProfile.LinkedInProfile,
                GitHubProfile = userProfile.GitHubProfile,
                PortfolioWebsite = userProfile.PortfolioWebsite,
                CreatedAt = userProfile.CreatedAt,
                UpdatedAt = userProfile.UpdatedAt,
                Educations = userProfile.Educations.Select(MapToDto).ToList(),
                WorkExperiences = userProfile.WorkExperiences.Select(MapToDto).ToList(),
                Skills = userProfile.Skills.Select(MapToDto).ToList(),
                Certifications = userProfile.Certifications.Select(MapToDto).ToList(),
                Projects = userProfile.Projects.Select(MapToDto).ToList()
            };
        }

        private static EducationDto MapToDto(Education education)
        {
            return new EducationDto
            {
                Id = education.Id,
                Institution = education.Institution,
                Degree = education.Degree,
                FieldOfStudy = education.FieldOfStudy,
                Grade = education.Grade,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
                IsCurrentlyStudying = education.IsCurrentlyStudying,
                Description = education.Description,
                Location = education.Location
            };
        }

        private static WorkExperienceDto MapToDto(WorkExperience workExperience)
        {
            return new WorkExperienceDto
            {
                Id = workExperience.Id,
                CompanyName = workExperience.CompanyName,
                JobTitle = workExperience.JobTitle,
                Location = workExperience.Location,
                StartDate = workExperience.StartDate,
                EndDate = workExperience.EndDate,
                IsCurrentlyWorking = workExperience.IsCurrentlyWorking,
                Description = workExperience.Description,
                Achievements = workExperience.Achievements,
                Department = workExperience.Department,
                EmploymentType = workExperience.EmploymentType
            };
        }

        private static SkillDto MapToDto(Skill skill)
        {
            return new SkillDto
            {
                Id = skill.Id,
                Name = skill.Name,
                Category = skill.Category,
                ProficiencyLevel = skill.ProficiencyLevel,
                YearsOfExperience = skill.YearsOfExperience,
                Description = skill.Description,
                IsHighlighted = skill.IsHighlighted
            };
        }

        private static CertificationDto MapToDto(Certification certification)
        {
            return new CertificationDto
            {
                Id = certification.Id,
                Name = certification.Name,
                IssuingOrganization = certification.IssuingOrganization,
                IssueDate = certification.IssueDate,
                ExpirationDate = certification.ExpirationDate,
                CredentialId = certification.CredentialId,
                CredentialUrl = certification.CredentialUrl,
                Description = certification.Description,
                IsCurrentlyValid = certification.IsCurrentlyValid
            };
        }

        private static ProjectDto MapToDto(Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Technologies = project.Technologies,
                ProjectUrl = project.ProjectUrl,
                GitHubUrl = project.GitHubUrl,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                IsCurrentlyWorking = project.IsCurrentlyWorking,
                Role = project.Role,
                Achievements = project.Achievements
            };
        }
    }
}
