using System.ComponentModel.DataAnnotations;

namespace AutomatedCVSystem.Core.DTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string Email { get; set; } = string.Empty;
        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string? PhoneNumber { get; set; }
        
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string? Address { get; set; }
        
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        public string? City { get; set; }
        
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
        public string? Country { get; set; }
        
        [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters")]
        public string? PostalCode { get; set; }
        
        [StringLength(1000, ErrorMessage = "Professional summary cannot exceed 1000 characters")]
        public string? ProfessionalSummary { get; set; }
        
        [Url(ErrorMessage = "Invalid LinkedIn profile URL")]
        [StringLength(1000, ErrorMessage = "LinkedIn profile URL cannot exceed 1000 characters")]
        public string? LinkedInProfile { get; set; }
        
        [Url(ErrorMessage = "Invalid GitHub profile URL")]
        [StringLength(1000, ErrorMessage = "GitHub profile URL cannot exceed 1000 characters")]
        public string? GitHubProfile { get; set; }
        
        [Url(ErrorMessage = "Invalid portfolio website URL")]
        [StringLength(1000, ErrorMessage = "Portfolio website URL cannot exceed 1000 characters")]
        public string? PortfolioWebsite { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public List<EducationDto> Educations { get; set; } = new();
        public List<WorkExperienceDto> WorkExperiences { get; set; } = new();
        public List<SkillDto> Skills { get; set; } = new();
        public List<CertificationDto> Certifications { get; set; } = new();
        public List<ProjectDto> Projects { get; set; } = new();
    }
    
    public class EducationDto
    {
        public int Id { get; set; }
        public string Institution { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string? FieldOfStudy { get; set; }
        public string? Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentlyStudying { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
    }
    
    public class WorkExperienceDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentlyWorking { get; set; }
        public string? Description { get; set; }
        public string? Achievements { get; set; }
        public string? Department { get; set; }
        public string? EmploymentType { get; set; }
    }
    
    public class SkillDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? ProficiencyLevel { get; set; }
        public int YearsOfExperience { get; set; }
        public string? Description { get; set; }
        public bool IsHighlighted { get; set; }
    }
    
    public class CertificationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? IssuingOrganization { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? CredentialId { get; set; }
        public string? CredentialUrl { get; set; }
        public string? Description { get; set; }
        public bool IsCurrentlyValid { get; set; }
    }
    
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Technologies { get; set; }
        public string? ProjectUrl { get; set; }
        public string? GitHubUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentlyWorking { get; set; }
        public string? Role { get; set; }
        public string? Achievements { get; set; }
    }
}
