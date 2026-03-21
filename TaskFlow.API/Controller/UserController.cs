using Microsoft.AspNetCore.Mvc;
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
        public UsersController(RegisterUserCommandHandler registerUserCommandHandler, GetUsersQueryHandler getUsersQueryHandler, 
            GetUserByIdQueryHandler getUserByIdQueryHandler, UpdateUserEmailCommandHandler updateUserEmailCommandHandler)
        {
            _registerUserCommandHandler = registerUserCommandHandler;
            _getUsersHandler = getUsersQueryHandler;
            _getUserByIdHandler = getUserByIdQueryHandler;
            _updateUserEmailCommandHandler = updateUserEmailCommandHandler;
            _updateUserEmailCommandHandler = updateUserEmailCommandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            try
            {
                var user = await _registerUserCommandHandler.Handle(command);

                return CreatedAtAction(nameof(Register),
                    new { id = user.Id },
                    new { user.Id, user.Name, Email = user.Email.Value });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _getUsersHandler.Handle();
            return Ok(users);
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
            try
            {
                var command = new UpdateUserEmailCommand(id, request.Email);
                var user = await _updateUserEmailCommandHandler.Handle(command);

                if (user == null)
                    return NotFound(new { message = "User not found." });

                return Ok(new { user.Id, user.Name, Email = user.Email.Value });
            }
            catch(InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}