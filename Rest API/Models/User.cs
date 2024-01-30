using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Rest_API.Models
{
    public partial class User
    {
        public User()
        {
            Images = new HashSet<Image>();
            Projects = new HashSet<Project>();
            Tasks = new HashSet<Task>();
        }

        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(50, ErrorMessage = "FirstName must not exceed 255 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [StringLength(50, ErrorMessage = "LastName must not exceed 255 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        [StringLength(100, ErrorMessage = "UserName must not exceed 255 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be either 'Male' or 'Female'")]
        public string Gender { get; set; }

        [Range(0, 150, ErrorMessage = "Age must be between 0 and 150")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(15, ErrorMessage = "Password must not exceed 15 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [StringLength(50, ErrorMessage = "Role must not exceed 50 characters")]
        public string Role { get; set; }
        public int? IsDeleted { get; set; }

        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
