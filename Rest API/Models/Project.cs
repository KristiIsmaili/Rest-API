using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Rest_API.Models
{
    public partial class Project
    {
        public Project()
        {
            Tasks = new HashSet<Task>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "ProjectName is required")]
        [StringLength(255, ErrorMessage = "ProjectName must not exceed 255 characters")]
        public string ProjectName { get; set; }

        [StringLength(1000, ErrorMessage = "ProjectDescription must not exceed 1000 characters")]
        public string ProjectDescription { get; set; }

        [Required(ErrorMessage = "CreationDate is required")]
        public DateTime? CreationDate { get; set; }

        [ForeignKey("User")]
        public int? ProjectAdmin { get; set; }

        public int? ProjectIsDeleted { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual User ProjectAdminNavigation { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
