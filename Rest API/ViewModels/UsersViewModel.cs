using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_API.ViewModels
{
    
        public class UserViewModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Gender { get; set; }
            public int? Age { get; set; }
            public string Role { get; set; }
        }

        public class UserCreateDto
        {
            [Required(ErrorMessage = "Firstname is required")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Lastname is required")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Username is required")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Gender is required")]
            public string Gender { get; set; }

            [Required(ErrorMessage = "Age is required")]
            public int? Age { get; set; }

            [Required(ErrorMessage = "Password is required")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Role is required")]
            public string Role { get; set; }
        }

    public class UserUpdateDto
    {
        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(50, ErrorMessage = "FirstName cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [StringLength(50, ErrorMessage = "LastName cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        [StringLength(50, ErrorMessage = "UserName cannot exceed 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(10, ErrorMessage = "Gender must be either Male or Female")]
        public string Gender { get; set; }

        [Range(0, 100, ErrorMessage = "Age must be between 0 and 100")]
        public int? Age { get; set; }
    }




}
