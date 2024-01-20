using System;
using System.Collections.Generic;

#nullable disable

namespace Rest_API.Models
{
    public partial class Project
    {
        public Project()
        {
            Tasks = new HashSet<Task>();
        }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ProjectAdmin { get; set; }

        public virtual User ProjectAdminNavigation { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
