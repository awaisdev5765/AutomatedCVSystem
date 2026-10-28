using AutomatedCVSystem.Core.DTOs;
using AutomatedCVSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutomatedCVSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobAnalysisController : ControllerBase
    {
        private readonly IJobAnalysisService _jobAnalysisService;
        private readonly ILogger<JobAnalysisController> _logger;

        public JobAnalysisController(IJobAnalysisService jobAnalysisService, ILogger<JobAnalysisController> logger)
        {
            _jobAnalysisService = jobAnalysisService;
            _logger = logger;
        }

        [HttpPost("analyze/{userProfileId}")]
        public async Task<ActionResult<JobAnalysisResult>> AnalyzeJobDescription(int userProfileId, JobAnalysisRequest request)
        {
            try
            {
                var result = await _jobAnalysisService.AnalyzeJobDescriptionAsync(userProfileId, request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing job description for user {UserId}", userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("extract-skills")]
        public async Task<ActionResult<List<string>>> ExtractSkills([FromBody] string jobDescription)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jobDescription))
                    return BadRequest("Job description cannot be empty");

                var skills = await _jobAnalysisService.ExtractSkillsAsync(jobDescription);
                return Ok(skills);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting skills from job description");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("extract-requirements")]
        public async Task<ActionResult<List<string>>> ExtractRequirements([FromBody] string jobDescription)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jobDescription))
                    return BadRequest("Job description cannot be empty");

                var requirements = await _jobAnalysisService.ExtractRequirementsAsync(jobDescription);
                return Ok(requirements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting requirements from job description");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("extract-experience")]
        public async Task<ActionResult<List<string>>> ExtractExperienceRequirements([FromBody] string jobDescription)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jobDescription))
                    return BadRequest("Job description cannot be empty");

                var experience = await _jobAnalysisService.ExtractExperienceRequirementsAsync(jobDescription);
                return Ok(experience);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting experience requirements from job description");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("extract-education")]
        public async Task<ActionResult<List<string>>> ExtractEducationRequirements([FromBody] string jobDescription)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jobDescription))
                    return BadRequest("Job description cannot be empty");

                var education = await _jobAnalysisService.ExtractEducationRequirementsAsync(jobDescription);
                return Ok(education);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting education requirements from job description");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("calculate-matching-score/{userProfileId}")]
        public async Task<ActionResult<MatchingScore>> CalculateMatchingScore(int userProfileId, JobAnalysisRequest request)
        {
            try
            {
                var matchingScore = await _jobAnalysisService.CalculateMatchingScoreAsync(userProfileId, request);
                return Ok(matchingScore);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating matching score for user {UserId}", userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
