using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rest_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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



        //Read
        [HttpGet]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IEnumerable<Task>> GetAsync()
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return null;
            }
            var userClaims = identity.Claims;

            var usrEmail = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;

            var usr = await _dbContext.Users.Where(user => user.IsDeleted == 0 && user.Email == usrEmail)
                            .FirstOrDefaultAsync();

            var userID = usr.UserId;

            var taskToGet = _dbContext.Tasks.Where(task => task.TaskIsDeleted == 0 && task.AssignedUserId == userID).Select(tsk => new Task
            {
                ProjectName = tsk.ProjectName,
                TaskName = tsk.TaskName,
                TaskDescription = tsk.TaskDescription,
                AssignetEmployee = tsk.AssignetEmployee,
                CreationDate = tsk.CreationDate,
                DueDate = tsk.DueDate,
                IsCompleted = tsk.IsCompleted
            }).ToList();

            return taskToGet; 
            
        }



        //Create
        [HttpPost("Create/task")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<Task>> Post([FromBody] taskCreationDto newTask)
        {
            if (newTask == null)
            {
                return BadRequest("Invalid data");
            }

            var empID = newTask.AssignedUserId;

            var usr = await _dbContext.Users
                            .Where(user => user.IsDeleted == 0 && user.UserId == empID)
                            .FirstOrDefaultAsync();

            var prjID = await _dbContext.Projects
                            .Where(prj => prj.ProjectIsDeleted == 0 && prj.ProjectName == newTask.ProjectName)
                            .FirstOrDefaultAsync();

            if (usr == null) {

                return BadRequest("There is no user with this ID, or the user is not assignet to this project");

            };

            if (prjID == null)
            {

                return BadRequest("There is no project with this name, or you are not assignet to this project");

            };

            var empName = usr.UserName;

            var projectID = prjID.ProjectId;

            var taskToAdd = new Task
            {
                ProjectName = newTask.ProjectName,
                ProjectId = projectID,
                TaskName = newTask.TaskName,
                TaskDescription = newTask.TaskDescription,
                AssignedUserId = newTask.AssignedUserId,
                AssignetEmployee = empName,
                CreationDate = newTask.CreationDate,
                DueDate = newTask.DueDate,
                IsCompleted = "No",
                TaskIsDeleted = 0,
            };

            _dbContext.Tasks.Add(taskToAdd);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAsync), new { id = taskToAdd.TaskId }, taskToAdd);
        }





        //Update
        [HttpPut("update/task/{taskNameI}")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> UpdateTasks(string taskNameI, [FromBody] taskUpdateDto taskUpdate)
        {
            try
            {


                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null)
                {
                    return null;
                }
                var userClaims = identity.Claims;

                var usrEmail = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;

                var usr = await _dbContext.Users.Where(user => user.IsDeleted == 0 && user.Email == usrEmail)
                                .FirstOrDefaultAsync();

                var userID = usr.UserId;






                var task = await _dbContext.Tasks.FirstOrDefaultAsync(u => u.TaskName == taskNameI);

                var taskUserID = task.AssignedUserId;

                if (userID != taskUserID)
                {
                    return BadRequest($"You can not change this task");
                }


                if (task == null)
                {
                    return NotFound($"There is no project with this name: ");
                }

                // Update task properties based on the provided DTO
                task.TaskDescription = taskUpdate.TaskDescription ?? task.TaskDescription;
                task.AssignedUserId = taskUpdate.AssignedUserId ?? task.AssignedUserId;
               


                // Update the task entity in the database
                _dbContext.Tasks.Update(task);
                await _dbContext.SaveChangesAsync();

                return Ok($"Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the project: {ex.Message}");
            }
        }




        //Task Completion Update
        [HttpPut("update/CompleteTask/{taskNameI}")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> UpdateCompleteTasks(string taskNameI, [FromBody] completeTaskDto taskUpdate)
        {
            try
            {


                //var identity = HttpContext.User.Identity as ClaimsIdentity;
                //if (identity == null)
                //{
                //    return null;
                //}
                //var userClaims = identity.Claims;

                //var usrEmail = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;

                //var usr = await _dbContext.Users.Where(user => user.IsDeleted == 0 && user.Email == usrEmail)
                //                .FirstOrDefaultAsync();

                //var userID = usr.UserId;




                var task = await _dbContext.Tasks.FirstOrDefaultAsync(u => u.TaskName == taskNameI);


                if (task == null)
                {
                    return NotFound($"There is no project with this name: ");
                }

                // Update task properties based on the provided DTO
                task.IsCompleted = taskUpdate.IsCompleted ?? task.TaskDescription;
               



                // Update the task entity in the database
                _dbContext.Tasks.Update(task);
                await _dbContext.SaveChangesAsync();

                return Ok($"Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the project: {ex.Message}");
            }
        }



        //Delete
        [HttpDelete("Delete/Task/{TaskName}")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> DeleteTask(string TaskName)
        {
            try
            {
                var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.TaskName == TaskName);

                if (task == null)
                {
                    return NotFound($"Could not find task with the name: {TaskName} ");
                }

                // Soft delete the project by setting projectIsDeleted to true
                task.TaskIsDeleted = 1;

                // Update the prj entity in the database
                _dbContext.Tasks.Update(task);
                await _dbContext.SaveChangesAsync();

                return Ok($"Project Deleted succefuly"); // HTTP 204 No Content response
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the project: {ex.Message}");
            }
        }



    }




    public class taskCreationDto
    {
        public string ProjectName { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public int? AssignedUserId { get; set; }
        //public string AssignetEmployee { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? DueDate { get; set; }
        
        

    }


    public class taskUpdateDto
    {
        
        public string TaskDescription { get; set; }
        public int? AssignedUserId { get; set; }
        

    }

    public class completeTaskDto
    {

        public string IsCompleted { get; set; }


    }





}
