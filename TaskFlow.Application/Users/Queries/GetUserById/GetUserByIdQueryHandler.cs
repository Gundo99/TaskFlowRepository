using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<GetUserByIdResponse?> Handle(Guid id)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
                return null;

            return new GetUserByIdResponse(
                user.Id,
                user.Name,
                user.Email.Value);
        }
    }
}
