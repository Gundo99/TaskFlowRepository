using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Users.Commands;
using TaskFlow.Domain.Users;

namespace TaskFlow.Application.Users.Handlers
{
    public class UpdateUserEmailCommandHandler
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserEmailCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> Handle(UpdateUserEmailCommand command)
        {
            var user = await _userRepository.GetById(command.UserId);

            if(user is null)
                return null;

            var existingUserWithEmail = await _userRepository.GetByEmail(command.Email);

            if (existingUserWithEmail != null && existingUserWithEmail.Id != user.Id)
                throw new InvalidOperationException("A user with this email already exists.");

            var email = new Email(command.Email);
            user.UpdateEmail(email);

            await _userRepository.Update(user);

            return user;
        }
    }
}
