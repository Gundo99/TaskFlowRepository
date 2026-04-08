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
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private IPasswordHasher _passwordHasher;

        public LoginUserCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator,
                IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Handle(
            LoginUserCommand loginUserCommand,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmail(loginUserCommand.Email);
            if (user == null) 
                throw new Exception("Invalid credentials.");

            var isValid = _passwordHasher.Verify(loginUserCommand.Password, user.PasswordHash);

            if (!isValid)
                throw new Exception("Invalid credentials.");

            return _jwtTokenGenerator.GenerateToken(user.Id, user.Email.Value);
        }
    }
}
