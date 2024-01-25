using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rest_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rest_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly kreatxTestContext _dbContext;

        public UserController(kreatxTestContext dbContext)
        {
            _dbContext = dbContext;
        }



        [HttpGet("Admins")]
        [Authorize(Roles = "Admin")]
        public IActionResult adminEndpoint()
        {
            var currentuser = GetCurrentUser();
            if(currentuser == null)
            {
                return Ok($"Empty");
            }
            return Ok($"Hi {currentuser.FirstName}, you are an {currentuser.Role}");
        }


        [HttpGet("Users")]
        [Authorize(Roles = "User")]
        public IActionResult userEndpoint()
        {
            var currentuser = GetCurrentUser();
            if (currentuser == null)
            {
                return Ok($"Empty");
            }
            return Ok($"Hi {currentuser.FirstName}, you are an {currentuser.Role}");
        }


        [HttpGet("AdminsAndUsers")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult adminAndUserEndpoint()
        {
            var currentuser = GetCurrentUser();
            if (currentuser == null)
            {
                return Ok($"Empty");
            }
            return Ok($"Hi {currentuser.FirstName}, you are an {currentuser.Role}");
        }



        [HttpGet("Public")]
        public IActionResult Public()
        {
            return Ok("Hi, you're on public property");
        }

        private User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if(identity != null)
            {
                var userClaims = identity.Claims;

                return new User
                {
                    UserName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    FirstName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    LastName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                };
            }
            return null;
        }


        
    }
}
