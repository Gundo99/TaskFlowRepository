using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.Contracts.Users
{
    public class RegisterUserRequest
    {
        [Required]
        [StringLength(100, MinimumLength =2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; }
    }
}
