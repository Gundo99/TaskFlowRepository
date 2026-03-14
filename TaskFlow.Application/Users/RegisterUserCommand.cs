using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Users
{
    public class RegisterUserCommand
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
