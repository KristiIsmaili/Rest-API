using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_API.ViewModels
{
    public class TasksViewModel
    {

        public class taskCreationDto
        {
            public string ProjectName { get; set; }
            public string TaskName { get; set; }
            public string TaskDescription { get; set; }
            public int? AssignedUserId { get; set; }
            public DateTime? CreationDate { get; set; }
            public DateTime? DueDate { get; set; }



        }


        public class taskUpdateDto
        {

            public string TaskDescription { get; set; }
            public int? AssignedUserId { get; set; }


        }

        public class completeTaskDto
        {

            public string IsCompleted { get; set; }


        }

    }
}
