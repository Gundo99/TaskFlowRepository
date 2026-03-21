using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Users.Commands;
using TaskFlow.Domain.Users;

namespace TaskFlow.Application.Users.Handlers
{
    public class DeleteUserCommandHandler
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteUserCommand command)
        {
            var user = await _userRepository.GetById(command.UserId);

            if (user is null)
                return false;

            await _userRepository.Delete(user);

            return true;
        }
    }
}
