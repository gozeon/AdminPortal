using System.ComponentModel.DataAnnotations;

namespace AdminPortal.Models
{
    public class Permission
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+\.[a-zA-Z]+$", ErrorMessage = "必须是英文+点号，例如 Order.View")]
        public string Name { get; set; } = null!;
        [Required]
        public string DisplayName { get; set; } = null!;
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "必须是英文，例如 Order")]
        public string Group { get; set; } = null!;
        public bool IsEnabled { get; set; } = true;
    }
}
