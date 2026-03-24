using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.Contracts
{
    public class UpdateTaskRequest
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }
    }
}
