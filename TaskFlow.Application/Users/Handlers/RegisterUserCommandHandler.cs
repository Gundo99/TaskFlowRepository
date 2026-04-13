using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Application.Users.Queries.GetUsers;
using TaskFlow.Domain.Users;
using TaskFlow.Domain.Users.Commands;


namespace TaskFlow.Application.Users.Handlers
{
    public class RegisterUserCommandHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserResponse> Handle(
            RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmail(request.Email);

            if (existingUser != null)
                throw new InvalidOperationException("User already exists");

            var email = new Email(request.Email);

            var passwordHash = _passwordHasher.Hash(request.Password);

            var user = new User(request.Name, email, passwordHash);

            await _userRepository.Add(user);

            return user.ToUserResponse();
        }
    }
}