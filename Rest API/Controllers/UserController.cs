using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rest_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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



        //[HttpGet("Admins")]
        //[Authorize(Roles = "Admin")]
        //public IActionResult adminEndpoint()
        //{
        //    var currentuser = GetCurrentUser();
        //    if(currentuser == null)
        //    {
        //        return Ok($"Empty");
        //    }
        //    return Ok($"Hi {currentuser.FirstName}, you are an {currentuser.Role}");
        //}


        //[HttpGet("Users")]
        //[Authorize(Roles = "Employee")]
        //public IActionResult userEndpoint()
        //{
        //    var currentuser = GetCurrentUser();
        //    if (currentuser == null)
        //    {
        //        return Ok($"Empty");
        //    }
        //    return Ok($"Hi {currentuser.FirstName}, you are an {currentuser.Role}");
        //}


        //[HttpGet("AdminAndEmployee")]
        //[Authorize(Roles = "Admin, Employee")]
        //public IActionResult adminAndUserEndpoint()
        //{
        //    var currentuser = GetCurrentUser();
        //    if (currentuser == null)
        //    {
        //        return Ok($"Empty");
        //    }
        //    return Ok($"Hi {currentuser.FirstName}, you are an {currentuser.Role}");
        //}



        //Read
        [HttpGet]
        [Authorize(Roles = "Admin, Employee")]
        public IEnumerable<User> Get()
        {

            var usr = _dbContext.Users.Where(user => user.IsDeleted == 0).Select(user => new User
            { FirstName = user.FirstName, LastName = user.LastName, UserName = user.UserName, Email = user.Email, Gender = user.Gender,
            Age = user.Age, Role = user.Role}).ToList();

            return usr;

        }


        //Create
        [HttpPost("Admin/add/User")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> Post([FromBody] UserCreateDto newUser)
        {
            if (newUser == null)
            {
                return BadRequest("Invalid user data");
            }

            var userToAdd = new User
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                UserName = newUser.UserName,
                Email = newUser.Email,
                Password = newUser.Password,
                Gender = newUser.Gender,
                Age = newUser.Age,
                Role = newUser.Role
            };

            _dbContext.Users.Add(userToAdd);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = userToAdd.UserId}, userToAdd);
        }


        //Update
        [HttpPut("update/user/profile")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UserUpdateDto userUpdateDto)
        {
            try
            {
                // Retrieve the user's ID from the authentication token
                var currentUser = GetCurrentUser();

                // Retrieve the user from the database based on the user's ID
                // var user = await _dbContext.Users.FindAsync(currentUser.Email);

                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == currentUser.Email);


                if (user == null)
                {
                    return NotFound($"User with email {currentUser.Email} not found.");
                }

                // Update user properties based on the provided DTO
                user.FirstName = userUpdateDto.FirstName ?? user.FirstName;
                user.LastName = userUpdateDto.LastName ?? user.LastName;
                user.UserName = userUpdateDto.UserName ?? user.UserName;
                user.Gender = userUpdateDto.Gender ?? user.Gender;
                user.Age = userUpdateDto.Age ?? user.Age;

                // Update the user entity in the database
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                return Ok($"Updated Successfully"); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the user: {ex.Message}");
            }
        }



        //Delete
        [HttpDelete("Admin/delete/users/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                // Soft delete the user by setting IsDeleted to true
                user.IsDeleted = 1;

                // Update the user entity in the database
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                return NoContent(); // HTTP 204 No Content response
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the user: {ex.Message}");
            }
        }




        //[HttpGet("Public")]
        //public IActionResult Public()
        //{
        //    return Ok("Hi, you're on public property");
        //}

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

    public class UserCreateDto
    {
        [Required(ErrorMessage = "Firstname is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Age is required")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }



    }

    public class UserUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
       
    }

}
