using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Users.Handlers;
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

        public UsersController(RegisterUserCommandHandler registerUserCommandHandler, GetUsersQueryHandler getUsersQueryHandler)
        {
            _registerUserCommandHandler = registerUserCommandHandler;
            _getUsersHandler = getUsersQueryHandler;
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
    }
}