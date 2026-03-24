using Microsoft.AspNetCore.Mvc;
using TaskFlow.API.Contracts.Tasks;
using TaskFlow.Application.Tasks.Commands;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskFlow.Application.Users.Commands;
using TaskFlow.Application.Users.Handlers;
using TaskFlow.Application.Users.Queries.GetTasksByUserId;

namespace TaskFlow.API.Controllers
{
    [ApiController]
    [Route("api/users/{userId:guid}/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly CreateTaskCommandHandler _createTaskHandler;
        private readonly GetTasksByUserIdQueryHandler _getTasksByUserIdQueryHandler;
        private readonly CompleteTaskCommdandHandler _completeTaskCommandHandler;

        public TasksController(CreateTaskCommandHandler createTaskHandler, GetTasksByUserIdQueryHandler getTasksByUserIdQuery
            , CompleteTaskCommdandHandler completeTaskCommdandHandler)
        {
            _createTaskHandler = createTaskHandler;
            _getTasksByUserIdQueryHandler = getTasksByUserIdQuery;
            _completeTaskCommandHandler = completeTaskCommdandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid userId, [FromBody] CreateTaskRequest request)
        {
            var command = new CreateTaskCommand(userId, request.Title, request.Description);
            var task = await _createTaskHandler.Handle(command);

            return Ok(task);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasksByUserId(Guid userId)
        {
            var query = new GetTaskByUserIdQuery(userId);
            var tasks = await _getTasksByUserIdQueryHandler.Handle(query);
            return Ok(tasks);
        }

        [HttpPatch("/api/tasks/{taskId:guid}/complete")]
        public async Task<IActionResult> CompleteTask(Guid taskId)
        {
            var command = new CompleteTaskCommand(taskId);
            await _completeTaskCommandHandler.Handle(command);
            return NoContent();
        }
    }
}