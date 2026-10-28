using AutomatedCVSystem.Core.DTOs;
using AutomatedCVSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutomatedCVSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfilesController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly ILogger<UserProfilesController> _logger;

        public UserProfilesController(IUserProfileService userProfileService, ILogger<UserProfilesController> logger)
        {
            _userProfileService = userProfileService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserProfileDto>>> GetAllUserProfiles()
        {
            try
            {
                var userProfiles = await _userProfileService.GetAllUserProfilesAsync();
                return Ok(userProfiles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all user profiles");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile(int id)
        {
            try
            {
                var userProfile = await _userProfileService.GetUserProfileAsync(id);
                if (userProfile == null)
                    return NotFound();

                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile {UserId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserProfileDto>> CreateUserProfile(UserProfileDto userProfileDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdUserProfile = await _userProfileService.CreateUserProfileAsync(userProfileDto);
                return CreatedAtAction(nameof(GetUserProfile), new { id = createdUserProfile.Id }, createdUserProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user profile");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserProfileDto>> UpdateUserProfile(int id, UserProfileDto userProfileDto)
        {
            try
            {
                if (id != userProfileDto.Id)
                    return BadRequest("ID mismatch");

                var updatedUserProfile = await _userProfileService.UpdateUserProfileAsync(id, userProfileDto);
                return Ok(updatedUserProfile);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user profile {UserId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserProfile(int id)
        {
            try
            {
                var result = await _userProfileService.DeleteUserProfileAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user profile {UserId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{userProfileId}/educations")]
        public async Task<ActionResult<EducationDto>> AddEducation(int userProfileId, EducationDto educationDto)
        {
            try
            {
                var education = await _userProfileService.AddEducationAsync(userProfileId, educationDto);
                return CreatedAtAction(nameof(GetUserProfile), new { id = userProfileId }, education);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding education for user {UserId}", userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{userProfileId}/educations/{educationId}")]
        public async Task<ActionResult<EducationDto>> UpdateEducation(int userProfileId, int educationId, EducationDto educationDto)
        {
            try
            {
                var education = await _userProfileService.UpdateEducationAsync(userProfileId, educationId, educationDto);
                return Ok(education);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating education {EducationId} for user {UserId}", educationId, userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{userProfileId}/educations/{educationId}")]
        public async Task<ActionResult> DeleteEducation(int userProfileId, int educationId)
        {
            try
            {
                var result = await _userProfileService.DeleteEducationAsync(userProfileId, educationId);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting education {EducationId} for user {UserId}", educationId, userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{userProfileId}/work-experiences")]
        public async Task<ActionResult<WorkExperienceDto>> AddWorkExperience(int userProfileId, WorkExperienceDto workExperienceDto)
        {
            try
            {
                var workExperience = await _userProfileService.AddWorkExperienceAsync(userProfileId, workExperienceDto);
                return CreatedAtAction(nameof(GetUserProfile), new { id = userProfileId }, workExperience);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding work experience for user {UserId}", userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{userProfileId}/work-experiences/{workExperienceId}")]
        public async Task<ActionResult<WorkExperienceDto>> UpdateWorkExperience(int userProfileId, int workExperienceId, WorkExperienceDto workExperienceDto)
        {
            try
            {
                var workExperience = await _userProfileService.UpdateWorkExperienceAsync(userProfileId, workExperienceId, workExperienceDto);
                return Ok(workExperience);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating work experience {WorkExperienceId} for user {UserId}", workExperienceId, userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{userProfileId}/work-experiences/{workExperienceId}")]
        public async Task<ActionResult> DeleteWorkExperience(int userProfileId, int workExperienceId)
        {
            try
            {
                var result = await _userProfileService.DeleteWorkExperienceAsync(userProfileId, workExperienceId);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting work experience {WorkExperienceId} for user {UserId}", workExperienceId, userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{userProfileId}/skills")]
        public async Task<ActionResult<SkillDto>> AddSkill(int userProfileId, SkillDto skillDto)
        {
            try
            {
                var skill = await _userProfileService.AddSkillAsync(userProfileId, skillDto);
                return CreatedAtAction(nameof(GetUserProfile), new { id = userProfileId }, skill);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding skill for user {UserId}", userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{userProfileId}/skills/{skillId}")]
        public async Task<ActionResult<SkillDto>> UpdateSkill(int userProfileId, int skillId, SkillDto skillDto)
        {
            try
            {
                var skill = await _userProfileService.UpdateSkillAsync(userProfileId, skillId, skillDto);
                return Ok(skill);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating skill {SkillId} for user {UserId}", skillId, userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{userProfileId}/skills/{skillId}")]
        public async Task<ActionResult> DeleteSkill(int userProfileId, int skillId)
        {
            try
            {
                var result = await _userProfileService.DeleteSkillAsync(userProfileId, skillId);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting skill {SkillId} for user {UserId}", skillId, userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{userProfileId}/certifications")]
        public async Task<ActionResult<CertificationDto>> AddCertification(int userProfileId, CertificationDto certificationDto)
        {
            try
            {
                var certification = await _userProfileService.AddCertificationAsync(userProfileId, certificationDto);
                return CreatedAtAction(nameof(GetUserProfile), new { id = userProfileId }, certification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding certification for user {UserId}", userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{userProfileId}/certifications/{certificationId}")]
        public async Task<ActionResult<CertificationDto>> UpdateCertification(int userProfileId, int certificationId, CertificationDto certificationDto)
        {
            try
            {
                var certification = await _userProfileService.UpdateCertificationAsync(userProfileId, certificationId, certificationDto);
                return Ok(certification);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating certification {CertificationId} for user {UserId}", certificationId, userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{userProfileId}/certifications/{certificationId}")]
        public async Task<ActionResult> DeleteCertification(int userProfileId, int certificationId)
        {
            try
            {
                var result = await _userProfileService.DeleteCertificationAsync(userProfileId, certificationId);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting certification {CertificationId} for user {UserId}", certificationId, userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{userProfileId}/projects")]
        public async Task<ActionResult<ProjectDto>> AddProject(int userProfileId, ProjectDto projectDto)
        {
            try
            {
                var project = await _userProfileService.AddProjectAsync(userProfileId, projectDto);
                return CreatedAtAction(nameof(GetUserProfile), new { id = userProfileId }, project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding project for user {UserId}", userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{userProfileId}/projects/{projectId}")]
        public async Task<ActionResult<ProjectDto>> UpdateProject(int userProfileId, int projectId, ProjectDto projectDto)
        {
            try
            {
                var project = await _userProfileService.UpdateProjectAsync(userProfileId, projectId, projectDto);
                return Ok(project);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project {ProjectId} for user {UserId}", projectId, userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{userProfileId}/projects/{projectId}")]
        public async Task<ActionResult> DeleteProject(int userProfileId, int projectId)
        {
            try
            {
                var result = await _userProfileService.DeleteProjectAsync(userProfileId, projectId);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project {ProjectId} for user {UserId}", projectId, userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
