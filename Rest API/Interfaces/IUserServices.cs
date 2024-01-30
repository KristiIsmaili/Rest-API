using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rest_API.Models;
using Rest_API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_API.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAllActiveUsers();
        Task<User> AddUserAsync(UserViewModel.UserCreateDto newUser);
        Task<IActionResult> UploadProfilePictureAsync(IFormFile file);
        Task<IActionResult> GetProfilePictureAsync();
        Task<IActionResult> UpdateProfilePictureAsync(IFormFile file);
        Task<IActionResult> UpdateUserProfileAsync(UserViewModel.UserUpdateDto userUpdateDto);
        Task<IActionResult> DeleteUserAsync(int userId);
    }
}
