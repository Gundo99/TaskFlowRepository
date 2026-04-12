using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Common.Response;

namespace TaskFlow.Application.Users.Commands
{
    public record  RefreshTokenCommand(string Token) : IRequest<AuthResponse>;
}
