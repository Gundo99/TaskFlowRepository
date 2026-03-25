using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Tasks;
using TaskFlow.Domain.Tasks;
using TaskFlow.Domain.Users;

namespace TaskFlow.Application.Users.Queries.GetTasksByUserId
{
    public class GetTasksByUserIdQueryHandler
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public GetTasksByUserIdQueryHandler(
            ITaskRepository taskRepository,
            IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<List<TaskFlow.Application.Tasks.TaskResponse>> Handle(GetTaskByUserIdQuery getTasksByUserIdQuery)
        {
            var user = await _userRepository.GetById(getTasksByUserIdQuery.UserId);

            if (user is null)
                throw new NotFoundException("User not found.");

            var tasks = await _taskRepository.GetByUserId(getTasksByUserIdQuery.UserId);

            return tasks.Select(t => t.ToTaskResponse()).ToList();
        }
    }
}
