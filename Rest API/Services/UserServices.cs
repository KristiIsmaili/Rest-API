using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rest_API.Interfaces;
using Rest_API.Models;
using Rest_API.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rest_API.Services
{
    public class UserServices : IUserService
    {

        private readonly kreatxTestContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserServices(kreatxTestContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }





        public IEnumerable<User> GetAllActiveUsers()
        {
            return _dbContext.Users.Where(user => user.IsDeleted == 0).ToList();
        }


        public async Task<User> AddUserAsync(UserViewModel.UserCreateDto newUser)
        {
            if (newUser == null)
            {
                throw new ArgumentNullException(nameof(newUser), "Invalid user data");
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
                Role = newUser.Role,
                IsDeleted = 0

            };

            _dbContext.Users.Add(userToAdd);
            await _dbContext.SaveChangesAsync();

            return userToAdd;
        }


        public async Task<IActionResult> UploadProfilePictureAsync(IFormFile file)
        {

            var httpContext = _httpContextAccessor.HttpContext;

            if (file == null || file.Length == 0)
            {
                return new BadRequestObjectResult("No file uploaded");
            }

            var user = GetCurrentUser(httpContext);
            var userInfo = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (userInfo == null)
            {
                return new NotFoundObjectResult("User not found");
            }

            var userID = userInfo.UserId;

            int imageCount = await _dbContext.Images.CountAsync(p => p.UserId == userID);

            if (imageCount >= 1)
            {
                return new BadRequestObjectResult("You already have a profile picture. You can't add another, but you can change it.");
            }

            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            var picture = new Image
            {
                ImageData = fileBytes,
                FileName = file.FileName,
                ContentType = file.ContentType,
                UserId = userID
            };

            _dbContext.Images.Add(picture);
            await _dbContext.SaveChangesAsync();

            return new OkObjectResult("File uploaded successfully");
        }


        public async Task<IActionResult> GetProfilePictureAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var user = GetCurrentUser(httpContext);

            if (user == null)
            {
                return new BadRequestObjectResult("User not found");
            }

            var userInfo = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (userInfo == null)
            {
                return new NotFoundObjectResult("User not found in the database");
            }

            var userID = userInfo.UserId;

            var picture = await _dbContext.Images.FirstOrDefaultAsync(i => i.UserId == userID);
            if (picture == null)
            {
                return new OkObjectResult("You do not have a profile picture.");
            }

            return new FileStreamResult(new MemoryStream(picture.ImageData), picture.ContentType);
        }


        public async Task<IActionResult> UpdateProfilePictureAsync(IFormFile file)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var user = GetCurrentUser(httpContext);

            if (user == null)
            {
                return new BadRequestObjectResult("User not found");
            }

            var userInfo = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (userInfo == null)
            {
                return new NotFoundObjectResult("User not found in the database");
            }

            var userID = userInfo.UserId;

            var picture = await _dbContext.Images.FirstOrDefaultAsync(i => i.UserId == userID);
            if (picture == null)
            {
                return new NotFoundObjectResult("Profile picture not found");
            }

            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            // Update the picture data
            picture.ImageData = fileBytes;
            picture.FileName = file.FileName;
            picture.ContentType = file.ContentType;

            // Update the database
            _dbContext.Images.Update(picture);
            await _dbContext.SaveChangesAsync();

            return new OkObjectResult("Profile picture updated successfully");
        }


        public async Task<IActionResult> UpdateUserProfileAsync(UserViewModel.UserUpdateDto userUpdateDto)
        {
            try
            {

                var httpContext = _httpContextAccessor.HttpContext;

                var currentUser = GetCurrentUser(httpContext); // Implement GetCurrentUser method

                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == currentUser.Email);

                if (user == null)
                {
                    return new NotFoundObjectResult($"User with email {currentUser.Email} not found.");
                }

                user.FirstName = userUpdateDto.FirstName ?? user.FirstName;
                user.LastName = userUpdateDto.LastName ?? user.LastName;
                user.UserName = userUpdateDto.UserName ?? user.UserName;
                user.Gender = userUpdateDto.Gender ?? user.Gender;
                user.Age = userUpdateDto.Age ?? user.Age;

                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                return new OkObjectResult("Updated Successfully");
            }
            catch (Exception ex)
            {
                return new ObjectResult($"An error occurred while updating the user: {ex.Message}")
                {
                    StatusCode = 500
                };
            }
        }


        public async Task<IActionResult> AdminUpdateUserAsync(int userId, UserViewModel.UserCreateDto changeUser)
        {
            try
            {

                var user = await _dbContext.Users.FindAsync(userId);

                if (user == null)
                {
                    return new NotFoundObjectResult($"User with ID {userId} not found.");
                }

                user.FirstName = changeUser.FirstName ?? user.FirstName;
                user.LastName = changeUser.LastName ?? user.LastName;
                user.UserName = changeUser.UserName ?? user.UserName;
                user.Email = changeUser.Email ?? user.Email;
                user.Password = changeUser.Password ?? user.Password;
                user.Gender = changeUser.Gender ?? user.Gender;
                user.Age = changeUser.Age ?? user.Age;
                user.Role = changeUser.Role ?? user.Role;

                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                return new OkObjectResult("Updated Successfully");

            }
            catch (Exception ex)
            {
                return new ObjectResult($"An error occurred while updating the user: {ex.Message}")
                {
                    StatusCode = 500
                };
            }



        }



        public async Task<IActionResult> DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(userId);

                if (user == null)
                {
                    return new NotFoundObjectResult($"User with ID {userId} not found.");
                }

                user.IsDeleted = 1;

                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                return new OkObjectResult("User deleted successfully");
            }
            catch (Exception ex)
            {
                return new ObjectResult($"An error occurred while deleting the user: {ex.Message}")
                {
                    StatusCode = 500
                };
            }
        }


        private User GetCurrentUser(HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
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
