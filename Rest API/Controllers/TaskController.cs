using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rest_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = Rest_API.Models.Task;

namespace Rest_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly kreatxTestContext _dbContext;

        public TaskController(kreatxTestContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IEnumerable<Task> Get()
        {
            
                return _dbContext.Tasks.ToList();
            
        }
    }
}
