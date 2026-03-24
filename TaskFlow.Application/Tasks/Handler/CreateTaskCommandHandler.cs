using TaskFlow.Domain.Tasks;
using TaskFlow.Domain.Users;

namespace TaskFlow.Application.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public CreateTaskCommandHandler(
            ITaskRepository taskRepository,
            IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<TaskResponse> Handle(CreateTaskCommand command)
        {
            var user = await _userRepository.GetById(command.UserId);

            if (user is null)
                throw new ArgumentException("User not found.");

            var taskItem = new TaskItem(command.Title, command.Description, command.UserId);

            await _taskRepository.Add(taskItem);

            return new TaskResponse(
                taskItem.Id,
                taskItem.Title,
                taskItem.Description,
                taskItem.IsCompleted,
                taskItem.CreatedAt,
                taskItem.UserId);
        }
    }
}