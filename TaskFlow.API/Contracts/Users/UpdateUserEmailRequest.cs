using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.Contracts.Users
{
    public class UpdateUserEmailRequest
    {
        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;
    }
}
