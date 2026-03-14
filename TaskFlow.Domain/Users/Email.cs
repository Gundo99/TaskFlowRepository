using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Users
{
    public class Email 
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty.");

            if(!value.Contains("@") || !value.Contains("."))
                throw new ArgumentException("Email is not valid.");

            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
