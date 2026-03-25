using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Tasks.Commands;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Application.Tasks.Handler
{
    public class DeleteTaskCommandHandler
    {
        private readonly ITaskRepository _taskRepository;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task Handle(DeleteTaskCommand command)
        {
            var taskItem = await _taskRepository.GetById(command.TaskId);

            if (taskItem == null)
                throw new NotFoundException("Task not found.");

            await _taskRepository.Delete(taskItem);
        }
    }
}
