using System.ComponentModel.DataAnnotations;

namespace AutomatedCVSystem.Core.Models
{
    public class Project
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [StringLength(2000)]
        public string? Technologies { get; set; } // Comma-separated list of technologies used
        
        [StringLength(1000)]
        public string? ProjectUrl { get; set; }
        
        [StringLength(1000)]
        public string? GitHubUrl { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        public bool IsCurrentlyWorking { get; set; } = false;
        
        [StringLength(200)]
        public string? Role { get; set; } // Your role in the project
        
        [StringLength(2000)]
        public string? Achievements { get; set; }
        
        // Foreign key
        public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; } = null!;
    }
}
