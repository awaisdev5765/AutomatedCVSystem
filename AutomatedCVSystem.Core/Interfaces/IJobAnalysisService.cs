using AutomatedCVSystem.Core.DTOs;

namespace AutomatedCVSystem.Core.Interfaces
{
    public interface IJobAnalysisService
    {
        Task<JobAnalysisResult> AnalyzeJobDescriptionAsync(int userProfileId, JobAnalysisRequest request);
        Task<List<string>> ExtractSkillsAsync(string jobDescription);
        Task<List<string>> ExtractRequirementsAsync(string jobDescription);
        Task<List<string>> ExtractExperienceRequirementsAsync(string jobDescription);
        Task<List<string>> ExtractEducationRequirementsAsync(string jobDescription);
        Task<MatchingScore> CalculateMatchingScoreAsync(int userProfileId, JobAnalysisRequest request);
    }
}
