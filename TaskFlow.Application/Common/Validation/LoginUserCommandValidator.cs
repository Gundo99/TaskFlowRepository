using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Users.Commands;

namespace TaskFlow.Application.Common.Validation
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Email)
                .NotEmpty()
                 .MinimumLength(6);
        }
    }
}
