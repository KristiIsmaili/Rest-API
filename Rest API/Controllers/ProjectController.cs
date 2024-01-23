using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IEnumerable<Project> Get()
        {
            using (var context = new kreatxTestContext())
            {
               
                var prj = context.Projects.Select(project =>new Project
                {ProjectName = project.ProjectName, ProjectDescription = project.ProjectDescription }).ToList();

                return context.Projects.ToList();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Project>> Post([FromBody] Project project)
        {
            if (project == null)
            {
                return BadRequest("Invalid project data");
            }

            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = project.ProjectId }, project);
        }

    }
}
