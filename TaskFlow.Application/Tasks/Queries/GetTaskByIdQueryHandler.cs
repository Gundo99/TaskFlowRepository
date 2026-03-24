using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Application.Tasks.Queries
{
    public class GetTaskByIdQueryHandler
    {
        private readonly ITaskRepository _taskRepository;

        public GetTaskByIdQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskResponse> Handle(GetTaskByIdQuery query)
        {
            var taskItem = await _taskRepository.GetById(query.TaskId);
            if (taskItem == null)
                throw new ArgumentException("Task not found");
            return taskItem.ToTaskResponse();
        }
    }
}
