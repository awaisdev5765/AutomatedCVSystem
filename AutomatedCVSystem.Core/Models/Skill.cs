using System.ComponentModel.DataAnnotations;

namespace AutomatedCVSystem.Core.Models
{
    public class Skill
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? Category { get; set; } // Technical, Soft Skills, Languages, etc.
        
        [StringLength(20)]
        public string? ProficiencyLevel { get; set; } // Beginner, Intermediate, Advanced, Expert
        
        public int YearsOfExperience { get; set; } = 0;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public bool IsHighlighted { get; set; } = false; // For CV customization
        
        // Foreign key
        public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; } = null!;
    }
}
