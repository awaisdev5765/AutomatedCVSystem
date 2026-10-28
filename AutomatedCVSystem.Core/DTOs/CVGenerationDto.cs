using System.ComponentModel.DataAnnotations;

namespace AutomatedCVSystem.Core.DTOs
{
    public class CVGenerationRequest
    {
        [Required(ErrorMessage = "User profile ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "User profile ID must be greater than 0")]
        public int UserProfileId { get; set; }
        
        [Required(ErrorMessage = "Job description ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Job description ID must be greater than 0")]
        public int JobDescriptionId { get; set; }
        
        [Required(ErrorMessage = "Template type is required")]
        [RegularExpression("^(Professional|Modern|ATS-Friendly)$", ErrorMessage = "Template type must be Professional, Modern, or ATS-Friendly")]
        public string TemplateType { get; set; } = "Professional";
        
        [Required(ErrorMessage = "File format is required")]
        [RegularExpression("^(PDF|DOCX)$", ErrorMessage = "File format must be PDF or DOCX")]
        public string FileFormat { get; set; } = "PDF";
        
        public bool IncludePhoto { get; set; } = false;
        
        [MaxLength(50, ErrorMessage = "Cannot highlight more than 50 skills")]
        public List<string> HighlightedSkills { get; set; } = new();
        
        [MaxLength(20, ErrorMessage = "Cannot highlight more than 20 experiences")]
        public List<string> HighlightedExperience { get; set; } = new();
    }
    
    public class CVGenerationResult
    {
        public int GeneratedCVId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileFormat { get; set; } = string.Empty;
        public string TemplateType { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public MatchingScore MatchingScore { get; set; } = new();
        public DateTime GeneratedAt { get; set; }
    }
    
    public class CVTemplate
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsATSCompatible { get; set; }
        public List<string> SupportedFormats { get; set; } = new();
    }
}
