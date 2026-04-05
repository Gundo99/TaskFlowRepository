using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Tasks.Commands;
using TaskFlow.Domain.Users.Commands;

namespace TaskFlow.Application.Common.Validation
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.userId)
                .NotEmpty();

            RuleFor(x => x.title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.description)
                .MaximumLength(1000);
        }
    }
}
