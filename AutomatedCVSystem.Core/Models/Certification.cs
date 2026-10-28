using System.ComponentModel.DataAnnotations;

namespace AutomatedCVSystem.Core.Models
{
    public class Certification
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? IssuingOrganization { get; set; }
        
        public DateTime IssueDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        
        [StringLength(100)]
        public string? CredentialId { get; set; }
        
        [StringLength(1000)]
        public string? CredentialUrl { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public bool IsCurrentlyValid { get; set; } = true;
        
        // Foreign key
        public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; } = null!;
    }
}
