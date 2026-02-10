using System.ComponentModel.DataAnnotations;

namespace AdminPortal.Models
{
    public class UserVM
    {
        public string? Id { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;
        public List<string> Roles { get; set; } = new();
    }
}
