using System.ComponentModel.DataAnnotations;

namespace AdminPortal.Options
{
    public class AdminOption
    {
        public string AdminRoleName { get; set; } = "Admin";
        [Required]
        public required string AdminEmail { get; set; }
        public string AdminPassword { get; set; } = "Admin@1234";
    }
}
