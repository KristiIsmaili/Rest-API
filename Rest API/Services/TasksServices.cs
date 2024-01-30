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
using Task = Rest_API.Models.Task;

namespace Rest_API.Services
{
    public class TasksServices : ITasksServices
    {
        private readonly kreatxTestContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TasksServices(kreatxTestContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }





        public async Task<IEnumerable<Models.Task>> GetTasksForCurrentUserAsync()
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return null;
            }

            var userClaims = identity.Claims;
            var userEmail = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;

            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.IsDeleted == 0 && user.Email == userEmail);
            if (user == null)
            {
                return null;
            }

            var userID = user.UserId;

            var tasks = await _dbContext.Tasks
                .Where(task => task.TaskIsDeleted == 0 && task.AssignedUserId == userID)
                .Select(tsk => new Models.Task
                {
                    ProjectName = tsk.ProjectName,
                    TaskName = tsk.TaskName,
                    TaskDescription = tsk.TaskDescription,
                    AssignetEmployee = tsk.AssignetEmployee,
                    CreationDate = tsk.CreationDate,
                    DueDate = tsk.DueDate,
                    IsCompleted = tsk.IsCompleted
                })
                .ToListAsync();

            return tasks;
        }



        public async Task<ActionResult<Task>> CreateTaskAsync(TasksViewModel.taskCreationDto newTask)
        {
            if (newTask == null)
            {
                return new BadRequestResult();
            }

            var user = await _dbContext.Users
                            .Where(u => u.IsDeleted == 0 && u.UserId == newTask.AssignedUserId)
                            .FirstOrDefaultAsync();

            var project = await _dbContext.Projects
                            .Where(p => p.ProjectIsDeleted == 0 && p.ProjectName == newTask.ProjectName)
                            .FirstOrDefaultAsync();

            if (user == null || project == null)
            {
                return new BadRequestResult();
            }

            var assignedEmployeeName = user.UserName;
            var projectId = project.ProjectId;

            var taskToAdd = new Task
            {
                ProjectName = newTask.ProjectName,
                ProjectId = projectId,
                TaskName = newTask.TaskName,
                TaskDescription = newTask.TaskDescription,
                AssignedUserId = newTask.AssignedUserId,
                AssignetEmployee = assignedEmployeeName,
                CreationDate = newTask.CreationDate,
                DueDate = newTask.DueDate,
                IsCompleted = "No",
                TaskIsDeleted = 0
            };

            _dbContext.Tasks.Add(taskToAdd);
            await _dbContext.SaveChangesAsync();

            return new OkObjectResult("Task added successfully");
        }



        public async Task<IActionResult> UpdateTaskAsync(string taskName, TasksViewModel.taskUpdateDto taskUpdateDto)
        {
            try
            {
                var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null)
                {
                    return null;
                }
                var userClaims = identity.Claims;
                var usrEmail = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;

                var usr = await _dbContext.Users.FirstOrDefaultAsync(user => user.IsDeleted == 0 && user.Email == usrEmail);
                if (usr == null)
                {
                    return new BadRequestResult();
                }

                var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.TaskName == taskName);
                if (task == null)
                {
                    return new BadRequestResult();
                }

                if (usr.UserId != task.AssignedUserId)
                {
                    return new BadRequestResult();
                }

                // Update task properties based on the provided DTO
                task.TaskDescription = taskUpdateDto.TaskDescription ?? task.TaskDescription;
                task.AssignedUserId = taskUpdateDto.AssignedUserId ?? task.AssignedUserId;

                // Update the task entity in the database
                _dbContext.Tasks.Update(task);
                await _dbContext.SaveChangesAsync();

                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }



        public async Task<IActionResult> CompleteTaskAsync(string taskNameI, TasksViewModel.completeTaskDto taskUpdate)
        {
            try
            {
                var task = await _dbContext.Tasks.FirstOrDefaultAsync(u => u.TaskName == taskNameI);

                if (task == null)
                {
                    return new NotFoundObjectResult($"There is no project with the name: {taskNameI}");
                }

                task.IsCompleted = taskUpdate.IsCompleted ?? task.IsCompleted;

                _dbContext.Tasks.Update(task);
                await _dbContext.SaveChangesAsync();

                return new OkObjectResult($"Task '{taskNameI}' updated successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }



        public async Task<IActionResult> DeleteTaskAsync(string taskName)
        {
            try
            {
                var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.TaskName == taskName);

                if (task == null)
                {
                    return new NotFoundObjectResult($"Could not find task with the name: {taskName}");
                }

                // Soft delete the task by setting TaskIsDeleted to 1
                task.TaskIsDeleted = 1;

                // Update the task entity in the database
                _dbContext.Tasks.Update(task);
                await _dbContext.SaveChangesAsync();

                return new OkObjectResult($"Task '{taskName}' deleted successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

    }
}
