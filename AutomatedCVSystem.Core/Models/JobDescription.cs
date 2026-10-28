using System.ComponentModel.DataAnnotations;

namespace AutomatedCVSystem.Core.Models
{
    public class JobDescription
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string JobTitle { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Location { get; set; }
        
        [Required]
        public string FullDescription { get; set; } = string.Empty;
        
        [StringLength(2000)]
        public string? ExtractedSkills { get; set; } // JSON string of extracted skills
        
        [StringLength(2000)]
        public string? ExtractedRequirements { get; set; } // JSON string of extracted requirements
        
        [StringLength(1000)]
        public string? ExtractedExperience { get; set; } // JSON string of experience requirements
        
        [StringLength(1000)]
        public string? ExtractedEducation { get; set; } // JSON string of education requirements
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Foreign key
        public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; } = null!;
    }
}
