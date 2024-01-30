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
    public interface ITasksServices
    {
        Task<IEnumerable<Models.Task>> GetTasksForCurrentUserAsync();
        Task<ActionResult<Models.Task>> CreateTaskAsync(TasksViewModel.taskCreationDto newTask);
        Task<IActionResult> UpdateTaskAsync(string taskName, TasksViewModel.taskUpdateDto taskUpdateDto);
        Task<IActionResult> CompleteTaskAsync(string taskName, TasksViewModel.completeTaskDto taskUpdateDto);
        Task<IActionResult> DeleteTaskAsync(string taskName);
    }
}
