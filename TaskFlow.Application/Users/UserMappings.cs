using TaskFlow.Application.Users.Queries.GetUsers;
using TaskFlow.Domain.Users;

namespace TaskFlow.Application.Users
{
    public static class UserMappings
    {
        public static UserResponse ToUserResponse(this User user)
        {
            return new UserResponse
            (
                user.Id,
                user.Email.Value,
                user.Name
            );
        }
    }
}