using System;
using System.Collections.Generic;

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

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int? IsDeleted { get; set; }

        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
