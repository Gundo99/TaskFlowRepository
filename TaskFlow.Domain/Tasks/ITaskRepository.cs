using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Tasks
{
    public interface ITaskRepository
    {
        Task Add(TaskItem taskItem);
        Task<TaskItem?> GetById(Guid id);
        Task<List<TaskItem>> GetByUserId(Guid userId);
        Task Update(TaskItem taskItem);
        Task Delete(TaskItem taskItem);
    }
}
