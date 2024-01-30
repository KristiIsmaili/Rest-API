using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rest_API.Interfaces;
using Rest_API.Models;
using Rest_API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {

        private readonly kreatxTestContext _dbContext;
        private readonly IProjectsServices _projectsService;

        public ProjectController(kreatxTestContext dbContext, IProjectsServices projectsService)
        {
            _dbContext = dbContext;
            _projectsService = projectsService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Employee")]
        public IEnumerable<Project> Get()
        {
            return _projectsService.GetProjects();
        }


        //Read
        //[HttpGet]
        //[Authorize(Roles = "Admin, Employee")]
        //public IEnumerable<Project> Get()
        //{

        //    var pro = _dbContext.Projects.Where(prj => prj.ProjectIsDeleted == 0).Select(prj => new Project
        //    { ProjectName = prj.ProjectName, ProjectDescription = prj.ProjectDescription, CreationDate = prj.CreationDate, UpdateDate = prj.UpdateDate, ProjectAdmin = prj.ProjectAdmin}).ToList();

        //    return pro;

        //}


        [HttpPost("Admin/create/projects")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Project>> Post([FromBody] ProjectsViewModel.ProjectCreationDto newProject)
        {
            if (newProject == null)
            {
                return BadRequest("Invalid project data");
            }

            try
            {
                var createdProject = await _projectsService.CreateProjectAsync(newProject);
                return CreatedAtAction(nameof(Get), new { id = createdProject.ProjectId }, createdProject);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the project: {ex.Message}");
            }
        }


        //Create
        //[HttpPost("Admin/create/projects")]
        //[Authorize(Roles = "Admin")]
        //public async Task<ActionResult<Project>> Post([FromBody] projectCreationDto newProject)
        //{
        //    if (newProject == null)
        //    {
        //        return BadRequest("Invalid project data");
        //    }

        //    var projectToCreate = new Project
        //    {
        //        ProjectName = newProject.ProjectName,
        //        ProjectDescription = newProject.ProjectDescription,
        //        CreationDate = newProject.CreationDate,
        //        ProjectAdmin = newProject.ProjectAdmin,
        //        ProjectIsDeleted = 0,
        //    };

        //    _dbContext.Projects.Add(projectToCreate);
        //    await _dbContext.SaveChangesAsync();

        //    return CreatedAtAction(nameof(Get), new { id = projectToCreate.ProjectId }, projectToCreate);
        //}


        [HttpPut("update/project/{projectNameInput}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProjects(string projectNameInput, [FromBody] ProjectsViewModel.ProjectUpdateDto projectUpdate)
        {
            if (projectUpdate == null)
            {
                return BadRequest("Invalid project update data");
            }

            try
            {
                var isUpdated = await _projectsService.UpdateProjectAsync(projectNameInput, projectUpdate);
                if (isUpdated)
                {
                    return Ok("Updated Successfully");
                }
                else
                {
                    return NotFound($"There is no project with the name: {projectNameInput}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the project: {ex.Message}");
            }
        }




        //Update
        //[HttpPut("update/project/{projectNameInput}")]
        //[Authorize(Roles = "Admin, Employee")]
        //public async Task<IActionResult> UpdateProjects(string projectNameInput, [FromBody] projectUpdateDto projectUpdate)
        //{
        //    try
        //    {


        //        var project = await _dbContext.Projects.FirstOrDefaultAsync(u => u.ProjectName == projectNameInput);


        //        if (project == null)
        //        {
        //            return NotFound($"There is no project with this name: ");
        //        }

        //        // Update user properties based on the provided DTO
        //        project.ProjectName = projectUpdate.ProjectName ?? project.ProjectName;
        //        project.ProjectDescription = projectUpdate.ProjectDescription ?? project.ProjectDescription;
        //        project.UpdateDate = projectUpdate.UpdateDate ?? project.UpdateDate;
        //        project.ProjectAdmin = projectUpdate.ProjectAdmin ?? project.ProjectAdmin;



        //        // Update the user entity in the database
        //        _dbContext.Projects.Update(project);
        //        await _dbContext.SaveChangesAsync();

        //        return Ok($"Updated Successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred while updating the project: {ex.Message}");
        //    }
        //}

        [HttpDelete("Admin/delete/project/{prjName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProject(string prjName)
        {
            try
            {
                var isDeleted = await _projectsService.DeleteProjectAsync(prjName);
                if (isDeleted)
                {
                    return Ok("Project Deleted Successfully");
                }
                else
                {
                    return NotFound($"Project not deleted");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the project: {ex.Message}");
            }
        }


        //Delete
        //[HttpDelete("Admin/delete/project/{prjName}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> DeleteProject(string prjName)
        //{
        //    try
        //    {
        //        var prj = await _dbContext.Projects.FirstOrDefaultAsync(p => p.ProjectName == prjName);

        //        if (prj == null)
        //        {
        //            return NotFound($"Could not find project with the name: {prjName} ");
        //        }

        //        var prjID = prj.ProjectId;

        //        var openTaskCount = await _dbContext.Tasks.CountAsync(u => u.ProjectId == prjID && u.IsCompleted == "No" && u.TaskIsDeleted == 0);


        //        if (openTaskCount != 0)
        //        {
        //            return BadRequest("This Project can not be deletet because there are open uncompleted tasks.");
        //        }

        //        // Soft delete the project by setting projectIsDeleted to true
        //        prj.ProjectIsDeleted = 1;

        //        // Update the prj entity in the database
        //        _dbContext.Projects.Update(prj);
        //        await _dbContext.SaveChangesAsync();

        //        return Ok($"Project Deleted succefuly"); // HTTP 204 No Content response
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred while deleting the project: {ex.Message}");
        //    }
        //}



        //public class projectCreationDto
        //{
        //    public string ProjectName { get; set; }
        //    public string ProjectDescription { get; set; }
        //    public DateTime? CreationDate { get; set; }
        //    public int? ProjectAdmin { get; set; }


        //}


        //public class projectUpdateDto
        //{
        //    public string ProjectName { get; set; }
        //    public string ProjectDescription { get; set; }
        //    public DateTime? UpdateDate { get; set; }
        //    public int? ProjectAdmin { get; set; }


        //}




    }
}
