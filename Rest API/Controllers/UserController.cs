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
    public class UserController : ControllerBase
    {
        

        [HttpGet]
        public IEnumerable<User> Get()
        {
            using (var context = new kreatxTestContext())
            {
                return context.Users.ToList();
            }
        }
    }
}
