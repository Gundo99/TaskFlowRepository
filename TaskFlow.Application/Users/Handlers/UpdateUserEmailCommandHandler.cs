using MediatR;
using TaskFlow.Application.Users.Commands;
using TaskFlow.Application.Users.Queries.GetUsers;
using TaskFlow.Domain.Users;

namespace TaskFlow.Application.Users.Handlers
{
    public class UpdateUserEmailCommandHandler
        : IRequestHandler<UpdateUserEmailCommand, UserResponse>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserEmailCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> Handle(
            UpdateUserEmailCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.UserId);

            if (user is null)
                throw new Exception("User not found"); 

            var existingUserWithEmail = await _userRepository.GetByEmail(request.Email);

            if (existingUserWithEmail != null && existingUserWithEmail.Id != user.Id)
                throw new InvalidOperationException("A user with this email already exists.");

            var email = new Email(request.Email);
            user.UpdateEmail(email);

            await _userRepository.Update(user);

            return user.ToUserResponse();
        }
    }
}