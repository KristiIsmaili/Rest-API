using Microsoft.EntityFrameworkCore;
using Rest_API.Interfaces;
using Rest_API.Models;
using Rest_API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_API.Services
{
    public class ProjectsServices : IProjectsServices
    {
        private readonly kreatxTestContext _dbContext;

        public ProjectsServices(kreatxTestContext dbContext)
        {
            _dbContext = dbContext;
        }





        public IEnumerable<Project> GetProjects()
        {
            return _dbContext.Projects
                .Where(prj => prj.ProjectIsDeleted == 0)
                .Select(prj => new Project
                {
                    ProjectName = prj.ProjectName,
                    ProjectDescription = prj.ProjectDescription,
                    CreationDate = prj.CreationDate,
                    UpdateDate = prj.UpdateDate,
                    ProjectAdmin = prj.ProjectAdmin
                })
                .ToList();
        }



        public async Task<Project> CreateProjectAsync(ProjectsViewModel.ProjectCreationDto newProject)
        {
            if (newProject == null)
            {
                throw new ArgumentNullException(nameof(newProject));
            }

            var projectToCreate = new Project
            {
                ProjectName = newProject.ProjectName,
                ProjectDescription = newProject.ProjectDescription,
                CreationDate = newProject.CreationDate,
                ProjectAdmin = newProject.ProjectAdmin,
                ProjectIsDeleted = 0,
            };

            _dbContext.Projects.Add(projectToCreate);
            await _dbContext.SaveChangesAsync();

            return projectToCreate;
        }



        public async Task<bool> UpdateProjectAsync(string projectNameInput, ProjectsViewModel.ProjectUpdateDto projectUpdate)
        {
            if (projectUpdate == null)
            {
                throw new ArgumentNullException(nameof(projectUpdate));
            }

            var project = await _dbContext.Projects.FirstOrDefaultAsync(u => u.ProjectName == projectNameInput);

            if (project == null)
            {
                return false;
            }

            // Update project properties based on the provided DTO
            project.ProjectName = projectUpdate.ProjectName ?? project.ProjectName;
            project.ProjectDescription = projectUpdate.ProjectDescription ?? project.ProjectDescription;
            project.UpdateDate = projectUpdate.UpdateDate ?? project.UpdateDate;
            project.ProjectAdmin = projectUpdate.ProjectAdmin ?? project.ProjectAdmin;

            // Update the project entity in the database
            _dbContext.Projects.Update(project);
            await _dbContext.SaveChangesAsync();

            return true;
        }




        public async Task<bool> DeleteProjectAsync(string projectName)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.ProjectName == projectName);

            if (project == null)
            {
                return false;
            }

            var projectId = project.ProjectId;

            var openTaskCount = await _dbContext.Tasks.CountAsync(u => u.ProjectId == projectId && u.IsCompleted == "No" && u.TaskIsDeleted == 0);

            if (openTaskCount != 0)
            {
                return false;
            }

            // Soft delete the project by setting projectIsDeleted to true
            project.ProjectIsDeleted = 1;

            // Update the project entity in the database
            _dbContext.Projects.Update(project);
            await _dbContext.SaveChangesAsync();

            return true;
        }

    }
}
