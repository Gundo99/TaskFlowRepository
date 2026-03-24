using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Users.Commands;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Application.Users.Handlers
{
    public class CompleteTaskCommdandHandler
    {
        private readonly ITaskRepository _taskRepository;

        public CompleteTaskCommdandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task Handle(CompleteTaskCommand command)
        {
            var task = await _taskRepository.GetById(command.TaskId);
            if (task is null)
                throw new ArgumentException("Task not found.");
            task.MarkAsCompleted();
            await _taskRepository.Update(task);
        }
    }
}
