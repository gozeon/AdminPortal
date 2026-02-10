namespace AdminPortal.Models
{
    public class RolePermissionVM
    {
        public string PermissionName { get; set; } = default!;
        public string PermissionDisplayName { get; set; } = default!;
        public bool Selected { get; set; } = false;
    }
}
