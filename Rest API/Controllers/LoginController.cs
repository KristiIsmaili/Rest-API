using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rest_API.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using static Rest_API.EventHandler.LoginHandler;
using Abp.Events.Bus;
using Rest_API.EventHandler;

namespace Rest_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly kreatxTestContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IEventBus _eventBus;

        public LoginController(kreatxTestContext dbContext, IConfiguration configuration, IEventBus eventBus)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _eventBus = eventBus;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);
            
            if(user != null)
            {

                        // Publish UserLoggedInEvent
                var loggedInEvent = new UserLoggedInEvent
                {
                    Email = user.Email,
                    LoginTime = DateTime.UtcNow
                };

               // _eventBus.TriggerAsync(new UserLoggedInEvent(loggedInEventData));
                //_eventBus.Publish(loggedInEvent);


                var token = Generate(user);
                
                return Ok(token);
            }

            return NotFound("User not found");
        }

        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Authenticate(UserLogin userLogin)
        {
            var currentUser = _dbContext.Users.FirstOrDefault(o => o.Email == userLogin.Email
            && o.Password == userLogin.Password);

            if (currentUser != null) 
            {
                return currentUser;
            }

            return null;
                
        }



       
    }
}

