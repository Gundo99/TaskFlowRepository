using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Users;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskFlow.Application.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PagedResult<UserResponse>> Handle(GetUsersQuery getUsersQuery)
        {
            if(getUsersQuery.PageNumber < 1)
                throw new ArgumentException("Page number must be greater than 0.", nameof(getUsersQuery.PageNumber));

            if (getUsersQuery.PageSize < 1 || getUsersQuery.PageSize > 100)
                throw new ArgumentException("Page size must be between 1 and 100.", nameof(getUsersQuery.PageSize));

            var users = await _userRepository.GetPaged(getUsersQuery.PageNumber, getUsersQuery.PageSize, getUsersQuery.Search);
            var totalCount = await _userRepository.Count(getUsersQuery.Search);

            var items = users
                .Select(user => new UserResponse(
                    user.Id,
                    user.Name,
                    user.Email.Value))
                .ToList();

            return new PagedResult<UserResponse>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = getUsersQuery.PageNumber,
                PageSize = getUsersQuery.PageSize
            };
        }
        public async Task<List<UserResponse>> Handle()
        {
            var users = await _userRepository.GetAll();

            return users
                .Select( user => new UserResponse(
                    user.Id,
                    user.Name,
                    user.Email.Value))
                .ToList();
        }
    }
}
