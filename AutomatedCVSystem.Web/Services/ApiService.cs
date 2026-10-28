using AutomatedCVSystem.Core.DTOs;
using System.Text;
using System.Text.Json;

namespace AutomatedCVSystem.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        // User Profile methods
        public async Task<List<UserProfileDto>> GetUserProfilesAsync()
        {
            var response = await _httpClient.GetAsync("userprofiles");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<UserProfileDto>>(json, _jsonOptions) ?? new List<UserProfileDto>();
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(int id)
        {
            var response = await _httpClient.GetAsync($"userprofiles/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserProfileDto>(json, _jsonOptions);
        }

        public async Task<UserProfileDto> CreateUserProfileAsync(UserProfileDto userProfile)
        {
            var json = JsonSerializer.Serialize(userProfile, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("userprofiles", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserProfileDto>(responseJson, _jsonOptions)!;
        }

        public async Task<UserProfileDto> UpdateUserProfileAsync(int id, UserProfileDto userProfile)
        {
            var json = JsonSerializer.Serialize(userProfile, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"userprofiles/{id}", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserProfileDto>(responseJson, _jsonOptions)!;
        }

        public async Task<bool> DeleteUserProfileAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"userprofiles/{id}");
            return response.IsSuccessStatusCode;
        }

        // Education methods
        public async Task<EducationDto> AddEducationAsync(int userProfileId, EducationDto education)
        {
            var json = JsonSerializer.Serialize(education, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"userprofiles/{userProfileId}/educations", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<EducationDto>(responseJson, _jsonOptions)!;
        }

        public async Task<EducationDto> UpdateEducationAsync(int userProfileId, int educationId, EducationDto education)
        {
            var json = JsonSerializer.Serialize(education, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"userprofiles/{userProfileId}/educations/{educationId}", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<EducationDto>(responseJson, _jsonOptions)!;
        }

        public async Task<bool> DeleteEducationAsync(int userProfileId, int educationId)
        {
            var response = await _httpClient.DeleteAsync($"userprofiles/{userProfileId}/educations/{educationId}");
            return response.IsSuccessStatusCode;
        }

        // Work Experience methods
        public async Task<WorkExperienceDto> AddWorkExperienceAsync(int userProfileId, WorkExperienceDto workExperience)
        {
            var json = JsonSerializer.Serialize(workExperience, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"userprofiles/{userProfileId}/work-experiences", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WorkExperienceDto>(responseJson, _jsonOptions)!;
        }

        public async Task<WorkExperienceDto> UpdateWorkExperienceAsync(int userProfileId, int workExperienceId, WorkExperienceDto workExperience)
        {
            var json = JsonSerializer.Serialize(workExperience, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"userprofiles/{userProfileId}/work-experiences/{workExperienceId}", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WorkExperienceDto>(responseJson, _jsonOptions)!;
        }

        public async Task<bool> DeleteWorkExperienceAsync(int userProfileId, int workExperienceId)
        {
            var response = await _httpClient.DeleteAsync($"userprofiles/{userProfileId}/work-experiences/{workExperienceId}");
            return response.IsSuccessStatusCode;
        }

        // Skills methods
        public async Task<SkillDto> AddSkillAsync(int userProfileId, SkillDto skill)
        {
            var json = JsonSerializer.Serialize(skill, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"userprofiles/{userProfileId}/skills", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<SkillDto>(responseJson, _jsonOptions)!;
        }

        public async Task<SkillDto> UpdateSkillAsync(int userProfileId, int skillId, SkillDto skill)
        {
            var json = JsonSerializer.Serialize(skill, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"userprofiles/{userProfileId}/skills/{skillId}", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<SkillDto>(responseJson, _jsonOptions)!;
        }

        public async Task<bool> DeleteSkillAsync(int userProfileId, int skillId)
        {
            var response = await _httpClient.DeleteAsync($"userprofiles/{userProfileId}/skills/{skillId}");
            return response.IsSuccessStatusCode;
        }

        // Job Analysis methods
        public async Task<JobAnalysisResult> AnalyzeJobDescriptionAsync(int userProfileId, JobAnalysisRequest request)
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"jobanalysis/analyze/{userProfileId}", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<JobAnalysisResult>(responseJson, _jsonOptions)!;
        }

        public async Task<List<string>> ExtractSkillsAsync(string jobDescription)
        {
            var json = JsonSerializer.Serialize(jobDescription, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("jobanalysis/extract-skills", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(responseJson, _jsonOptions) ?? new List<string>();
        }

        // CV Generation methods
        public async Task<CVGenerationResult> GenerateCVAsync(CVGenerationRequest request)
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("cvgeneration/generate", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CVGenerationResult>(responseJson, _jsonOptions)!;
        }

        public async Task<byte[]> GeneratePDFAsync(CVGenerationRequest request)
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("cvgeneration/generate-pdf", content);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<byte[]> GenerateWordAsync(CVGenerationRequest request)
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("cvgeneration/generate-word", content);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<List<CVTemplate>> GetAvailableTemplatesAsync()
        {
            var response = await _httpClient.GetAsync("cvgeneration/templates");
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CVTemplate>>(responseJson, _jsonOptions) ?? new List<CVTemplate>();
        }

        public async Task<List<CVGenerationResult>> GetGeneratedCVsForUserAsync(int userProfileId)
        {
            var response = await _httpClient.GetAsync($"cvgeneration/user/{userProfileId}");
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CVGenerationResult>>(responseJson, _jsonOptions) ?? new List<CVGenerationResult>();
        }

        public async Task<bool> DeleteGeneratedCVAsync(int generatedCVId)
        {
            var response = await _httpClient.DeleteAsync($"cvgeneration/{generatedCVId}");
            return response.IsSuccessStatusCode;
        }
    }
}
