using System;
using System.Collections.Generic;

#nullable disable

namespace Rest_API.Models
{
    public partial class Role
    {
        public int RoleId { get; set; }
        public string EmployeeRole { get; set; }
        public int? EmployeeId { get; set; }

        public virtual User Employee { get; set; }
    }
}
