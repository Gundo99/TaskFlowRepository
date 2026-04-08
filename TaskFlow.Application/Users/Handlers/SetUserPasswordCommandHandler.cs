using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Application.Users.Commands;
using TaskFlow.Domain.Users;

namespace TaskFlow.Application.Users.Handlers
{
    public class SetUserPasswordCommandHandler : IRequestHandler<SetUserPasswordCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public SetUserPasswordCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(SetUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.UserId);

            if (user == null)
                throw new Exception("User not found");

            var hashedPassword = _passwordHasher.Hash(request.Password);

            user.SetPassword(hashedPassword);

            await _userRepository.Update(user);

            return Unit.Value;
        }
    }
}
