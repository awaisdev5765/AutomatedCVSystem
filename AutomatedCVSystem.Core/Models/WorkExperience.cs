using System.ComponentModel.DataAnnotations;

namespace AutomatedCVSystem.Core.Models
{
    public class WorkExperience
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string JobTitle { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Location { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        public bool IsCurrentlyWorking { get; set; } = false;
        
        [StringLength(2000)]
        public string? Description { get; set; }
        
        [StringLength(2000)]
        public string? Achievements { get; set; }
        
        [StringLength(200)]
        public string? Department { get; set; }
        
        [StringLength(100)]
        public string? EmploymentType { get; set; } // Full-time, Part-time, Contract, etc.
        
        // Foreign key
        public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; } = null!;
    }
}
