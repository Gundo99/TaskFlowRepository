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
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
    {
        public readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByRefreshTokenAsync(request.Token);
            if (user == null)
                throw new Exception("Invalid refresh token.");
            var refreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == request.Token);
            if (refreshToken == null || refreshToken.ExpiryDate < DateTime.UtcNow)
                throw new Exception("Invalid refresh token.");
            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken(user);
            user.RefreshTokens.Remove(refreshToken);
            user.RefreshTokens.Add(newRefreshToken);
            await _userRepository.Update(user);
            return new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            };
        }
    }
}
