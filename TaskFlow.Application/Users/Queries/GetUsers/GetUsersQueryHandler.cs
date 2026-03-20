using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Users;

namespace TaskFlow.Application.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserResponse>> Handle()
        {
            var users = await _userRepository.GetAll();

            return users
                .Select( user => new UserResponse(
                    user.Id,
                    user.Name,
                    user.Email.Value))
                .ToList();
        }
    }
}
