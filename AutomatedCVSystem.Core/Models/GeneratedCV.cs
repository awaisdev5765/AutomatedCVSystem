using System.ComponentModel.DataAnnotations;

namespace AutomatedCVSystem.Core.Models
{
    public class GeneratedCV
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string FileName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string FileFormat { get; set; } = string.Empty; // PDF, DOCX
        
        [Required]
        [StringLength(50)]
        public string TemplateType { get; set; } = string.Empty; // Professional, Modern, ATS-Friendly
        
        public string FilePath { get; set; } = string.Empty;
        
        public long FileSizeBytes { get; set; }
        
        [StringLength(2000)]
        public string? MatchingScore { get; set; } // JSON string with detailed matching scores
        
        [StringLength(2000)]
        public string? HighlightedSkills { get; set; } // JSON string of highlighted skills
        
        [StringLength(2000)]
        public string? HighlightedExperience { get; set; } // JSON string of highlighted experience
        
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        
        // Foreign keys
        public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; } = null!;
        
        public int JobDescriptionId { get; set; }
        public virtual JobDescription JobDescription { get; set; } = null!;
    }
}
