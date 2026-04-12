using MediatR;
using Microsoft.AspNetCore.Authorization;
using TaskFlow.API.Requests;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.API.Contracts.Users;
using TaskFlow.Application.Users;
using TaskFlow.Application.Users.Commands;
using TaskFlow.Application.Users.Handlers;
using TaskFlow.Application.Users.Queries.GetUserById;
using TaskFlow.Application.Users.Queries.GetUsers;
using TaskFlow.Domain.Users.Commands;
using Microsoft.AspNetCore.Identity.Data;
using LoginRequest = TaskFlow.API.Requests.LoginRequest;

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
        private readonly IMediator _mediator;
        public UsersController(RegisterUserCommandHandler registerUserCommandHandler, GetUsersQueryHandler getUsersQueryHandler,
            GetUserByIdQueryHandler getUserByIdQueryHandler, UpdateUserEmailCommandHandler updateUserEmailCommandHandler,
            DeleteUserCommandHandler deleteUserCommandHandler, IMediator mediator)
        {
            _registerUserCommandHandler = registerUserCommandHandler;
            _getUsersHandler = getUsersQueryHandler;
            _getUserByIdHandler = getUserByIdQueryHandler;
            _updateUserEmailCommandHandler = updateUserEmailCommandHandler;
            _updateUserEmailCommandHandler = updateUserEmailCommandHandler;
            _deleteUserCommandHandler = deleteUserCommandHandler;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _registerUserCommandHandler.Handle(new RegisterUserCommand(request.Name, request.Email, request.Password), cancellationToken);

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10, [FromQuery] string? search = null,
            [FromQuery] string? sortBy = "name", [FromQuery] string? sortDirection = "asc")
        {
            var query = new GetUsersQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SortBy = sortBy,
                SortDirection = sortDirection
            };

            var result = await _getUsersHandler.Handle(query);
            return Ok(result);

        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _getUserByIdHandler.Handle(id);

            if (user == null)
                return NotFound(new { message = "User not found." });

            return Ok(user);
        }

        [Authorize]
        [HttpPut("{id:guid}/email")]
        public async Task<IActionResult> UpdateEmail(Guid id, [FromBody] UpdateUserEmailRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateUserEmailCommand(id, request.Email);
            var user = await _updateUserEmailCommandHandler.Handle(command, cancellationToken);

            if (user == null)
                return NotFound(new { message = "User not found." });

            return Ok(new { user.Id, user.Name, Email = user.Email });
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteUserCommand(id);

            var success = await _deleteUserCommandHandler.Handle(command);

            if (!success)
                return NotFound(new { message = "User not found." });

            return NoContent();
        }
        [HttpPut("{id}/password")]
        public async Task<IActionResult> SetPassword(Guid id, [FromBody] SetPasswordRequest request)
        {
            await _mediator.Send(new SetUserPasswordCommand(id, request.Password));

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var token = await _mediator.Send(
                new LoginUserCommand(loginRequest.Email, loginRequest.Password));

            return Ok(new { Token = token });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var authResponse = await _mediator.Send(
                new RefreshTokenCommand(request.RefreshToken));
            return Ok(authResponse);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are admin");
        }
    }
}