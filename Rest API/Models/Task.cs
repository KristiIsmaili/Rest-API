using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Rest_API.Models
{
    public partial class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "TaskName is required")]
        [StringLength(255, ErrorMessage = "TaskName must not exceed 255 characters")]
        public string TaskName { get; set; }

        [StringLength(1000, ErrorMessage = "TaskDescription must not exceed 1000 characters")]
        public string TaskDescription { get; set; }

        [ForeignKey("Project")]
        public int? ProjectId { get; set; }

        [ForeignKey("User")]
        public int? AssignedUserId { get; set; }

        [Required(ErrorMessage = "IsCompleted is required")]
        public string IsCompleted { get; set; }

        [Required(ErrorMessage = "CreationDate is required")]
        public DateTime? CreationDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? TaskIsDeleted { get; set; }
        public string ProjectName { get; set; }
        public string AssignetEmployee { get; set; }

        public virtual User AssignedUser { get; set; }
        public virtual Project Project { get; set; }
    }
}
