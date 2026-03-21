using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Users.Handlers;
using TaskFlow.Application.Users.Queries.GetUserById;
using TaskFlow.Application.Users.Queries.GetUsers;
using TaskFlow.Domain.Users;
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

        public UsersController(RegisterUserCommandHandler registerUserCommandHandler, GetUsersQueryHandler getUsersQueryHandler, GetUserByIdQueryHandler getUserByIdQueryHandler)
        {
            _registerUserCommandHandler = registerUserCommandHandler;
            _getUsersHandler = getUsersQueryHandler;
            _getUserByIdHandler = getUserByIdQueryHandler;
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
    }
}