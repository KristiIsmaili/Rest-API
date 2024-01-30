using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_API.ViewModels
{
    public class ProjectsViewModel
    {
        public class ProjectCreationDto
        {
            public string ProjectName { get; set; }
            public string ProjectDescription { get; set; }
            public DateTime? CreationDate { get; set; }
            public int? ProjectAdmin { get; set; }


        }


        public class ProjectUpdateDto
        {
            public string ProjectName { get; set; }
            public string ProjectDescription { get; set; }
            public DateTime? UpdateDate { get; set; }
            public int? ProjectAdmin { get; set; }


        }
    }
}
