using TaskFlow.Domain.Users;

namespace TaskFlow.Application.Users
{
    public static class UserMappings
    {
        public static UserResponse ToUserResponse(this User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email.Value,
                Name = user.Name
            };
        }
    }
}