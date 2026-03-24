using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Tasks.Queries
{
    public class GetTaskByIdQuery
    {
        public Guid TaskId { get; }

        public GetTaskByIdQuery(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}
