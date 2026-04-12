using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Application.Common.Response;
using TaskFlow.Application.Users.Commands;
using TaskFlow.Domain.Users;

namespace TaskFlow.Application.Users.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private IPasswordHasher _passwordHasher;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LoginUserCommandHandler(IUserRepository userRepository, ITokenService tokenService,
                IPasswordHasher passwordHasher, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthResponse> Handle(
            LoginUserCommand loginUserCommand,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmail(loginUserCommand.Email);
            if (user == null) 
                throw new Exception("Invalid credentials.");

            var isValid = _passwordHasher.Verify(loginUserCommand.Password, user.PasswordHash);

            if (!isValid)
                throw new Exception("Invalid credentials.");

           var accessToken = _tokenService.GenerateAccessToken(user);
           var refreshToken = _tokenService.GenerateRefreshToken(user);

            await _refreshTokenRepository.AddAsync(refreshToken);


            await _userRepository.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
    }
}
