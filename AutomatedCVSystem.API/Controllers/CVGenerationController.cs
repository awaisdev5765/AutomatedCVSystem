using AutomatedCVSystem.Core.DTOs;
using AutomatedCVSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutomatedCVSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CVGenerationController : ControllerBase
    {
        private readonly ICVGenerationService _cvGenerationService;
        private readonly ILogger<CVGenerationController> _logger;

        public CVGenerationController(ICVGenerationService cvGenerationService, ILogger<CVGenerationController> logger)
        {
            _cvGenerationService = cvGenerationService;
            _logger = logger;
        }

        [HttpPost("generate")]
        public async Task<ActionResult<CVGenerationResult>> GenerateCV(CVGenerationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _cvGenerationService.GenerateCVAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating CV for user {UserId}", request.UserProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("generate-pdf")]
        public async Task<ActionResult> GeneratePDF(CVGenerationRequest request)
        {
            try
            {
                var pdfBytes = await _cvGenerationService.GeneratePDFAsync(request);
                var fileName = $"{request.UserProfileId}_CV_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF CV for user {UserId}", request.UserProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("generate-word")]
        public async Task<ActionResult> GenerateWord(CVGenerationRequest request)
        {
            try
            {
                var wordBytes = await _cvGenerationService.GenerateWordAsync(request);
                var fileName = $"{request.UserProfileId}_CV_{DateTime.Now:yyyyMMdd_HHmmss}.docx";
                
                return File(wordBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Word CV for user {UserId}", request.UserProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("templates")]
        public async Task<ActionResult<List<CVTemplate>>> GetAvailableTemplates()
        {
            try
            {
                var templates = await _cvGenerationService.GetAvailableTemplatesAsync();
                return Ok(templates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available CV templates");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("generated/{generatedCVId}")]
        public async Task<ActionResult<CVGenerationResult>> GetGeneratedCV(int generatedCVId)
        {
            try
            {
                var result = await _cvGenerationService.GetGeneratedCVAsync(generatedCVId);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting generated CV {CVId}", generatedCVId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("user/{userProfileId}")]
        public async Task<ActionResult<List<CVGenerationResult>>> GetGeneratedCVsForUser(int userProfileId)
        {
            try
            {
                var results = await _cvGenerationService.GetGeneratedCVsForUserAsync(userProfileId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting generated CVs for user {UserId}", userProfileId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("generated/{generatedCVId}")]
        public async Task<ActionResult> DeleteGeneratedCV(int generatedCVId)
        {
            try
            {
                var result = await _cvGenerationService.DeleteGeneratedCVAsync(generatedCVId);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting generated CV {CVId}", generatedCVId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("download/{generatedCVId}")]
        public async Task<ActionResult> DownloadGeneratedCV(int generatedCVId)
        {
            try
            {
                var result = await _cvGenerationService.GetGeneratedCVAsync(generatedCVId);
                if (result == null)
                    return NotFound();

                if (!System.IO.File.Exists(result.FilePath))
                    return NotFound("File not found on disk");

                var fileBytes = await System.IO.File.ReadAllBytesAsync(result.FilePath);
                var contentType = result.FileFormat.ToUpper() == "PDF" 
                    ? "application/pdf" 
                    : "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

                return File(fileBytes, contentType, result.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading generated CV {CVId}", generatedCVId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
