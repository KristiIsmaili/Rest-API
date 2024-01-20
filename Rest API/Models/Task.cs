using System;
using System.Collections.Generic;

#nullable disable

namespace Rest_API.Models
{
    public partial class Task
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public int? ProjectId { get; set; }
        public int? AssignedUserId { get; set; }
        public string IsCompleted { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? DueDate { get; set; }

        public virtual User AssignedUser { get; set; }
        public virtual Project Project { get; set; }
    }
}
