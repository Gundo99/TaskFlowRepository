using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Users.Queries.GetUsers
{
    public class GetUsersQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public  string? Search { get; set; }
        public string? SortBy { get; set; } = "name";
        public string? SortDirection { get; set; } = "asc";
    }
}
