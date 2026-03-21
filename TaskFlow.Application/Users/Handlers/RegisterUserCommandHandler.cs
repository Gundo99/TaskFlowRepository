using TaskFlow.Domain.Users;
using TaskFlow.Domain.Users.Commands;


namespace TaskFlow.Application.Users.Handlers
{
    public class RegisterUserCommandHandler
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(RegisterUserCommand command)
        {
            var existingUser = await _userRepository.GetByEmail(command.Email);
            if (existingUser != null)
            {
                throw new Exception("Email is already in use.");
            }

            var email = new Email(command.Email);
            var user = new User(command.Name, email);

            await _userRepository.Add(user);

            return user;
        }
    }
}