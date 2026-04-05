using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Application.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler
    {
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskResponse> Handle(UpdateTaskCommand command)
        {
            var taskItem = await _taskRepository.GetById(command.taskId);

            if (taskItem is null)
                throw new NotFoundException("Task not found.");

            taskItem.UpdateDetails(command.title, command.description);

            await _taskRepository.Update(taskItem);

            return taskItem.ToTaskResponse();
        }
    }
}