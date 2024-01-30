using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rest_API.Interfaces;
using Rest_API.Models;
using Rest_API.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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

        private readonly IUserService _userService;

        public UserController(kreatxTestContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }




        //Read
        [HttpGet]
        [Authorize(Roles = "Admin, Employee")]
        public IEnumerable<User> Get()
        {
            return _userService.GetAllActiveUsers();
        }




        //Read
        //[HttpGet]
        //[Authorize(Roles = "Admin, Employee")]
        //public IEnumerable<User> Get()
        //{

        //    var usr = _dbContext.Users.Where(user => user.IsDeleted == 0).Select(user => new User
        //    {
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        UserName = user.UserName,
        //        Email = user.Email,
        //        Gender = user.Gender,
        //        Age = user.Age,
        //        Role = user.Role
        //    }).ToList();

        //    return usr;

        //}

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Create


        [HttpPost("Admin/add/User")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> Post([FromBody] UserViewModel.UserCreateDto newUser)
        {
            if (newUser == null)
            {
                return BadRequest("Invalid user data");
            }

            var user = await _userService.AddUserAsync(newUser);

            return CreatedAtAction(nameof(Get), new { id = user.UserId }, user);
        }


        ////Create
        //[HttpPost("Admin/add/User")]
        //[Authorize(Roles = "Admin")]
        //public async Task<ActionResult<User>> Post([FromBody] UserViewModel.UserCreateDto newUser)
        //{
        //    if (newUser == null)
        //    {
        //        return BadRequest("Invalid user data");
        //    }

        //    var userToAdd = new User
        //    {
        //        FirstName = newUser.FirstName,
        //        LastName = newUser.LastName,
        //        UserName = newUser.UserName,
        //        Email = newUser.Email,
        //        Password = newUser.Password,
        //        Gender = newUser.Gender,
        //        Age = newUser.Age,
        //        Role = newUser.Role,
        //        IsDeleted = 0
        //    };

        //    _dbContext.Users.Add(userToAdd);
        //    await _dbContext.SaveChangesAsync();

        //    return CreatedAtAction(nameof(Get), new { id = userToAdd.UserId }, userToAdd);
        //}

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Add Profile Picture


        [HttpPost("upload/profilePicture")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> UploadPicture(IFormFile file)
        {
            return await _userService.UploadProfilePictureAsync(file);
        }






        //Add Pofile Picture
        //[HttpPost("upload/profilePicture")]
        //[Authorize(Roles = "Admin, Employee")]
        //public async Task<IActionResult> UploadPicture(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return BadRequest("No file uploaded");
        //    }

        //    var user = GetCurrentUser();

        //    var userInfo = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

        //    var userID = userInfo.UserId;

        //    int imageCount = await _dbContext.Images
        //                               .CountAsync(p => p.UserId == userID);

        //    if (imageCount >= 1)
        //    {
        //        return BadRequest("You have a profile picture and can't add another but you can change it.");
        //    }

        //    byte[] fileBytes;
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        await file.CopyToAsync(memoryStream);
        //        fileBytes = memoryStream.ToArray();
        //    }


        //    var picture = new Image
        //    {
        //        ImageData = fileBytes,
        //        FileName = file.FileName,
        //        ContentType = file.ContentType,
        //        UserId = userID
        //    };

        //    _dbContext.Images.Add(picture);
        //    await _dbContext.SaveChangesAsync();

        //    return Ok("File uploaded successfully");
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Show Profile Picture



        [HttpGet("View/ProfilePicture")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> GetProfilePicture()
        {
            return await _userService.GetProfilePictureAsync();
        }




        //Show profile picture
        //[HttpGet("View/ProfilePicture")]
        //[Authorize(Roles = "Admin, Employee")]
        //public async Task<IActionResult> GetImage()
        //{

        //    var user = GetCurrentUser();

        //    var userInfo = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

        //    var userID = userInfo.UserId;


        //    var picture = await _dbContext.Images.FirstOrDefaultAsync(i => i.UserId == userID);
        //    if (picture == null)
        //    {
        //        return Ok($"You do not have a profile picture.");
        //    }

        //    return File(picture.ImageData, picture.ContentType);
        //}

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Update profile picture



        [HttpPut("upload/change/ProfilePicture")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> UpdatePicture(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            var result = await _userService.UpdateProfilePictureAsync(file);

            return result;
        }




        //Update profile picture
        //[HttpPut("upload/change/ProfilePicture")]
        //[Authorize(Roles = "Admin, Employee")]
        //public async Task<IActionResult> UpdatePicture(IFormFile file)
        //{




        //    var user = GetCurrentUser();

        //    var userInfo = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

        //    var userID = userInfo.UserId;

        //    var picture = await _dbContext.Images.FirstOrDefaultAsync(i=> i.UserId == userID);
        //    if (picture == null)
        //    {
        //        return NotFound();
        //    }


        //    byte[] fileBytes;
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        await file.CopyToAsync(memoryStream);
        //        fileBytes = memoryStream.ToArray();
        //    }

        //    // Update the picture data
        //    picture.ImageData = fileBytes;
        //    picture.FileName = file.FileName;
        //    picture.ContentType = file.ContentType;

        //    // Update the database
        //    _dbContext.Images.Update(picture);
        //    await _dbContext.SaveChangesAsync();

        //    return Ok("Picture updated successfully");
        //}

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //Update User Profile


        [HttpPut("update/user/profile")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UserViewModel.UserUpdateDto userUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.UpdateUserProfileAsync(userUpdateDto);

            return result;
        }



        ////Update User Profile
        //[HttpPut("update/user/profile")]
        //[Authorize(Roles = "Admin, Employee")]
        //public async Task<IActionResult> UpdateCurrentUser([FromBody] UserUpdateDto userUpdateDto)
        //{
        //    try
        //    {

        //        var currentUser = GetCurrentUser();
        //        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == currentUser.Email);


        //        if (user == null)
        //        {
        //            return NotFound($"User with email {currentUser.Email} not found.");
        //        }


        //        user.FirstName = userUpdateDto.FirstName ?? user.FirstName;
        //        user.LastName = userUpdateDto.LastName ?? user.LastName;
        //        user.UserName = userUpdateDto.UserName ?? user.UserName;
        //        user.Gender = userUpdateDto.Gender ?? user.Gender;
        //        user.Age = userUpdateDto.Age ?? user.Age;


        //        _dbContext.Users.Update(user);
        //        await _dbContext.SaveChangesAsync();

        //        return Ok($"Updated Successfully"); 
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred while updating the user: {ex.Message}");
        //    }
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        [HttpDelete("Admin/delete/users/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return result;
        }




        //[HttpDelete("Admin/delete/users/{id}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    try
        //    {
        //        var user = await _dbContext.Users.FindAsync(id);

        //        if (user == null)
        //        {
        //            return NotFound($"User with ID {id} not found.");
        //        }

                
        //        user.IsDeleted = 1;

                
        //        _dbContext.Users.Update(user);
        //        await _dbContext.SaveChangesAsync();

        //        return Ok("User deleted successfuly"); 
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred while deleting the user: {ex.Message}");
        //    }
        //}




        //private User GetCurrentUser()
        //{
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;

        //    if(identity != null)
        //    {
        //        var userClaims = identity.Claims;

        //        return new User
        //        {
        //            UserName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
        //            Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
        //            FirstName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
        //            LastName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
        //            Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
        //        };
        //    }
        //    return null;
        //}


        
    }

    //public class UserCreateDto
    //{
    //    [Required(ErrorMessage = "Firstname is required")]
    //    public string FirstName { get; set; }

    //    [Required(ErrorMessage = "Lastname is required")]
    //    public string LastName { get; set; }

    //    [Required(ErrorMessage = "Username is required")]
    //    public string UserName { get; set; }

    //    [Required(ErrorMessage = "Email is required")]
    //    [EmailAddress(ErrorMessage = "Invalid email format")]
    //    public string Email { get; set; }

    //    [Required(ErrorMessage = "Gender is required")]
    //    public string Gender { get; set; }

    //    [Required(ErrorMessage = "Age is required")]
    //    public int? Age { get; set; }

    //    [Required(ErrorMessage = "Password is required")]
    //    public string Password { get; set; }

    //    [Required(ErrorMessage = "Role is required")]
    //    public string Role { get; set; }



    //}

    //public class UserUpdateDto
    //{
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string UserName { get; set; }
    //    public string Gender { get; set; }
    //    public int? Age { get; set; }
       
    //}

}
