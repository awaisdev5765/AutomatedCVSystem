using AutomatedCVSystem.Core.DTOs;
using AutomatedCVSystem.Core.Models;

namespace AutomatedCVSystem.Core.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfileDto?> GetUserProfileAsync(int id);
        Task<UserProfileDto> CreateUserProfileAsync(UserProfileDto userProfile);
        Task<UserProfileDto> UpdateUserProfileAsync(int id, UserProfileDto userProfile);
        Task<bool> DeleteUserProfileAsync(int id);
        Task<List<UserProfileDto>> GetAllUserProfilesAsync();
        
        // Education methods
        Task<EducationDto> AddEducationAsync(int userProfileId, EducationDto education);
        Task<EducationDto> UpdateEducationAsync(int userProfileId, int educationId, EducationDto education);
        Task<bool> DeleteEducationAsync(int userProfileId, int educationId);
        
        // Work Experience methods
        Task<WorkExperienceDto> AddWorkExperienceAsync(int userProfileId, WorkExperienceDto workExperience);
        Task<WorkExperienceDto> UpdateWorkExperienceAsync(int userProfileId, int workExperienceId, WorkExperienceDto workExperience);
        Task<bool> DeleteWorkExperienceAsync(int userProfileId, int workExperienceId);
        
        // Skills methods
        Task<SkillDto> AddSkillAsync(int userProfileId, SkillDto skill);
        Task<SkillDto> UpdateSkillAsync(int userProfileId, int skillId, SkillDto skill);
        Task<bool> DeleteSkillAsync(int userProfileId, int skillId);
        
        // Certifications methods
        Task<CertificationDto> AddCertificationAsync(int userProfileId, CertificationDto certification);
        Task<CertificationDto> UpdateCertificationAsync(int userProfileId, int certificationId, CertificationDto certification);
        Task<bool> DeleteCertificationAsync(int userProfileId, int certificationId);
        
        // Projects methods
        Task<ProjectDto> AddProjectAsync(int userProfileId, ProjectDto project);
        Task<ProjectDto> UpdateProjectAsync(int userProfileId, int projectId, ProjectDto project);
        Task<bool> DeleteProjectAsync(int userProfileId, int projectId);
    }
}
