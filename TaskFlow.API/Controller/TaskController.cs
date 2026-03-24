using Microsoft.AspNetCore.Mvc;
using TaskFlow.API.Contracts.Tasks;
using TaskFlow.Application.Tasks.Commands;
using TaskFlow.Application.Tasks.Commands.CreateTask;

namespace TaskFlow.API.Controllers
{
    [ApiController]
    [Route("api/users/{userId:guid}/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly CreateTaskCommandHandler _createTaskHandler;

        public TasksController(CreateTaskCommandHandler createTaskHandler)
        {
            _createTaskHandler = createTaskHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid userId, [FromBody] CreateTaskRequest request)
        {
            var command = new CreateTaskCommand(userId, request.Title, request.Description);
            var task = await _createTaskHandler.Handle(command);

            return Ok(task);
        }
    }
}