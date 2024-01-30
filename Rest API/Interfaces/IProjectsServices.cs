using Rest_API.Models;
using Rest_API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_API.Interfaces
{
    public interface IProjectsServices
    {

        IEnumerable<Project> GetProjects();
        Task<Project> CreateProjectAsync(ProjectsViewModel.ProjectCreationDto newProject);
        Task<bool> UpdateProjectAsync(string projectNameInput, ProjectsViewModel.ProjectUpdateDto projectUpdate);
        Task<bool> DeleteProjectAsync(string projectName);

    }
}
