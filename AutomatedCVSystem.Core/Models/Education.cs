using System.ComponentModel.DataAnnotations;

namespace AutomatedCVSystem.Core.Models
{
    public class Education
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Institution { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Degree { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? FieldOfStudy { get; set; }
        
        [StringLength(100)]
        public string? Grade { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        public bool IsCurrentlyStudying { get; set; } = false;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [StringLength(100)]
        public string? Location { get; set; }
        
        // Foreign key
        public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; } = null!;
    }
}
