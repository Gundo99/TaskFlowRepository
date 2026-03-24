using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Users.Queries.GetTasksByUserId
{
    public class GetTaskByUserIdQuery
    {
        public Guid UserId { get; }

        public GetTaskByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
