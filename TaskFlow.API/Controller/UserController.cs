using Microsoft.AspNetCore.Mvc;
using TaskFlow.API.Contracts.Users;
using TaskFlow.Application.Users;
using TaskFlow.Application.Users.Commands;
using TaskFlow.Application.Users.Handlers;
using TaskFlow.Application.Users.Queries.GetUserById;
using TaskFlow.Application.Users.Queries.GetUsers;
using TaskFlow.Domain.Users.Commands;

namespace TaskFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly RegisterUserCommandHandler _registerUserCommandHandler;
        private readonly GetUsersQueryHandler _getUsersHandler;
        private readonly GetUserByIdQueryHandler _getUserByIdHandler;
        private readonly UpdateUserEmailCommandHandler _updateUserEmailCommandHandler;
        private readonly DeleteUserCommandHandler _deleteUserCommandHandler;
        public UsersController(RegisterUserCommandHandler registerUserCommandHandler, GetUsersQueryHandler getUsersQueryHandler,
            GetUserByIdQueryHandler getUserByIdQueryHandler, UpdateUserEmailCommandHandler updateUserEmailCommandHandler,
            DeleteUserCommandHandler deleteUserCommandHandler)
        {
            _registerUserCommandHandler = registerUserCommandHandler;
            _getUsersHandler = getUsersQueryHandler;
            _getUserByIdHandler = getUserByIdQueryHandler;
            _updateUserEmailCommandHandler = updateUserEmailCommandHandler;
            _updateUserEmailCommandHandler = updateUserEmailCommandHandler;
            _deleteUserCommandHandler = deleteUserCommandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var command = new RegisterUserCommand(request.Name, request.Email);
            var user = await _registerUserCommandHandler.Handle(command);

             return CreatedAtAction(nameof(Register),
                 new { id = user.Id },
                 new { user.Id, user.Name, Email = user.Email.Value }
             );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber =1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var query = new GetUsersQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search
            };

            var result = await _getUsersHandler.Handle(query);
            return Ok(result);

        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _getUserByIdHandler.Handle(id);

            if (user == null)
                return NotFound(new { message = "User not found." });

            return Ok(user);
        }

        [HttpPut("{id:guid}/email")]
        public async Task<IActionResult> UpdateEmail(Guid id, [FromBody] UpdateUserEmailRequest request)
        {
                var command = new UpdateUserEmailCommand(id, request.Email);
                var user = await _updateUserEmailCommandHandler.Handle(command);

                if (user == null)
                    return NotFound(new { message = "User not found." });

                return Ok(new { user.Id, user.Name, Email = user.Email.Value });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteUserCommand(id);

            var success = await _deleteUserCommandHandler.Handle(command);

            if (!success)
                return NotFound(new { message = "User not found." });

            return NoContent();
        }
    }
}