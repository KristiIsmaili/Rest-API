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
        

        [HttpGet]
        public IEnumerable<Task> Get()
        {
            using (var context = new kreatxTestContext())
            {
                return context.Tasks.ToList();
            }
        }
    }
}
