using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Users.Queries.GetUsers;
using TaskFlow.Domain.Users;

namespace TaskFlow.Application.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        } 

        public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.UserId);

            if (user == null)
                throw new Exception("User not found");

            return new UserResponse
            (
                user.Id,
                user.Email.Value,
                user.Name
            );

        }
    }
}
