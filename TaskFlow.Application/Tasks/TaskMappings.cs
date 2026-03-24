using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Tasks
{
    public static class TaskMappings
    {
        public static TaskResponse ToTaskResponse(this Domain.Tasks.TaskItem taskItem)
        {
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
