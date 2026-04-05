using MediatR;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Domain.Tasks;
using TaskFlow.Domain.Users;

namespace TaskFlow.Application.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskResponse>
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

        public async Task<TaskResponse> Handle(
            CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.userId);
            if (user == null)
                throw new NotFoundException($"User with id {request} not found.");

            var taskItem = new TaskItem(
                request.title,
                request.description,
                request.userId);
            await _taskRepository.Add(taskItem);

            return taskItem.ToTaskResponse();
        }
    }
}