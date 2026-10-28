namespace AutomatedCVSystem.Core.DTOs
{
    public class JobAnalysisRequest
    {
        public string JobDescription { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string? Location { get; set; }
    }
    
    public class JobAnalysisResult
    {
        public int JobDescriptionId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string? Location { get; set; }
        public List<string> ExtractedSkills { get; set; } = new();
        public List<string> ExtractedRequirements { get; set; } = new();
        public List<string> ExtractedExperience { get; set; } = new();
        public List<string> ExtractedEducation { get; set; } = new();
        public MatchingScore MatchingScore { get; set; } = new();
    }
    
    public class MatchingScore
    {
        public double OverallScore { get; set; }
        public double SkillsMatch { get; set; }
        public double ExperienceMatch { get; set; }
        public double EducationMatch { get; set; }
        public List<SkillMatch> SkillMatches { get; set; } = new();
        public List<ExperienceMatch> ExperienceMatches { get; set; } = new();
        public List<EducationMatch> EducationMatches { get; set; } = new();
    }
    
    public class SkillMatch
    {
        public string SkillName { get; set; } = string.Empty;
        public double MatchScore { get; set; }
        public bool IsExactMatch { get; set; }
        public bool IsHighlighted { get; set; }
    }
    
    public class ExperienceMatch
    {
        public string CompanyName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public double MatchScore { get; set; }
        public bool IsHighlighted { get; set; }
    }
    
    public class EducationMatch
    {
        public string Institution { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public double MatchScore { get; set; }
        public bool IsHighlighted { get; set; }
    }
}
