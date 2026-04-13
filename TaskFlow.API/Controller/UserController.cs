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
using TaskFlow.Application.Common.Response;

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
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var result = await _mediator.Send(
                new RegisterUserCommand(request.Name, request.Email, request.Password));

            return Ok(ApiResponse<UserResponse>.SuccessResponse(result));
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

            var result = await _mediator.Send(query);
            return Ok(result);

        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));

            if (user == null)
                return NotFound(new { message = "User not found." });

            return Ok(user);
        }

        [Authorize]
        [HttpPut("{id:guid}/email")]
        public async Task<IActionResult> UpdateEmail(Guid id, [FromBody] UpdateUserEmailRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send( new UpdateUserEmailCommand(id, request.Email));

            return Ok(ApiResponse<UserResponse>.SuccessResponse(result));
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteUserCommand(id));
            return Ok(ApiResponse<string>.SuccessResponse("Deleted successfully"));
        }
        [HttpPut("{id}/password")]
        public async Task<IActionResult> SetPassword(Guid id, [FromBody] SetPasswordRequest request)
        {
            await _mediator.Send(new SetUserPasswordCommand(id, request.Password));

            return Ok(ApiResponse<string>.SuccessResponse("Password set successfully"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _mediator.Send(
                new LoginUserCommand(loginRequest.Email, loginRequest.Password));

            return Ok(ApiResponse<AuthResponse>.SuccessResponse(result));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _mediator.Send(
                new RefreshTokenCommand(request.RefreshToken));

            return Ok(ApiResponse<AuthResponse>.SuccessResponse(result));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok(ApiResponse<string>.SuccessResponse("You are an admin"));
        }
    }
}