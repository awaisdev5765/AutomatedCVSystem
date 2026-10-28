using AutomatedCVSystem.Core.DTOs;

namespace AutomatedCVSystem.Core.Interfaces
{
    public interface ICVGenerationService
    {
        Task<CVGenerationResult> GenerateCVAsync(CVGenerationRequest request);
        Task<byte[]> GeneratePDFAsync(CVGenerationRequest request);
        Task<byte[]> GenerateWordAsync(CVGenerationRequest request);
        Task<List<CVTemplate>> GetAvailableTemplatesAsync();
        Task<CVGenerationResult?> GetGeneratedCVAsync(int generatedCVId);
        Task<List<CVGenerationResult>> GetGeneratedCVsForUserAsync(int userProfileId);
        Task<bool> DeleteGeneratedCVAsync(int generatedCVId);
    }
}
