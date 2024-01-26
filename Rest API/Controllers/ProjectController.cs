using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rest_API.Models;
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

        public ProjectController(kreatxTestContext dbContext)
        {
            _dbContext = dbContext;
        }

        //[HttpGet]
        //public IEnumerable<Project> Get()
        //{
             
        //        var prj = _dbContext.Projects.Select(project =>new Project
        //        {ProjectName = project.ProjectName, ProjectDescription = project.ProjectDescription }).ToList();

        //        return _dbContext.Projects.ToList();
            
        //}

        //[HttpPost]
        //public async Task<ActionResult<Project>> Post([FromBody] Project project)
        //{
        //    if (project == null)
        //    {
        //        return BadRequest("Invalid project data");
        //    }

        //    _dbContext.Projects.Add(project);
        //    await _dbContext.SaveChangesAsync();

        //    return CreatedAtAction(nameof(Get), new { id = project.ProjectId }, project);
        //}


        //Read
        [HttpGet]
        [Authorize(Roles = "Admin, Employee")]
        public IEnumerable<Project> Get()
        {
             
            var pro = _dbContext.Projects.Where(prj => prj.ProjectIsDeleted == 0).Select(prj => new Project
            { ProjectName = prj.ProjectName, ProjectDescription = prj.ProjectDescription, CreationDate = prj.CreationDate, UpdateDate = prj.UpdateDate, ProjectAdmin = prj.ProjectAdmin}).ToList();

            return pro;

        }


        //Create
        [HttpPost("Admin/create/projects")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Project>> Post([FromBody] projectCreationDto newProject)
        {
            if (newProject == null)
            {
                return BadRequest("Invalid project data");
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

            return CreatedAtAction(nameof(Get), new { id = projectToCreate.ProjectId }, projectToCreate);
        }






        //Update
        [HttpPut("update/project/{projectNameInput}")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> UpdateProjects(string projectNameInput, [FromBody] projectUpdateDto projectUpdate)
        {
            try
            {
                

                var project = await _dbContext.Projects.FirstOrDefaultAsync(u => u.ProjectName == projectNameInput);


                if (project == null)
                {
                    return NotFound($"There is no project with this name: ");
                }

                // Update user properties based on the provided DTO
                project.ProjectName = projectUpdate.ProjectName ?? project.ProjectName;
                project.ProjectDescription = projectUpdate.ProjectDescription ?? project.ProjectDescription;
                project.UpdateDate = projectUpdate.UpdateDate ?? project.UpdateDate;
                project.ProjectAdmin = projectUpdate.ProjectAdmin ?? project.ProjectAdmin;


            
                // Update the user entity in the database
                _dbContext.Projects.Update(project);
                await _dbContext.SaveChangesAsync();

                return Ok($"Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the project: {ex.Message}");
            }
        }




        //Delete
        [HttpDelete("Admin/delete/project/{prjName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProject(string prjName)
        {
            try
            {
                var prj = await _dbContext.Projects.FirstOrDefaultAsync(p => p.ProjectName == prjName);

                if (prj == null)
                {
                    return NotFound($"Could not find project with the name: {prjName} ");
                }

                // Soft delete the project by setting projectIsDeleted to true
                prj.ProjectIsDeleted = 1;

                // Update the prj entity in the database
                _dbContext.Projects.Update(prj);
                await _dbContext.SaveChangesAsync();

                return Ok($"Project Deleted succefuly"); // HTTP 204 No Content response
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the project: {ex.Message}");
            }
        }



        public class projectCreationDto
        {
            public string ProjectName { get; set; }
            public string ProjectDescription { get; set; }
            public DateTime? CreationDate { get; set; }
            public int? ProjectAdmin { get; set; }


        }


        public class projectUpdateDto
        {
            public string ProjectName { get; set; }
            public string ProjectDescription { get; set; }
            public DateTime? UpdateDate { get; set; }
            public int? ProjectAdmin { get; set; }


        }




    }
}
