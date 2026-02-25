using System.Security.Claims;
using AdminPortal.Data;
using AdminPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Areas.Identity.Pages.Role
{
    [Authorize(Policy = "Permission:Role.Edit")]
    public class EditModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;
        public EditModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext)
        {
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
        }

        [BindProperty]
        public IdentityRole Role { get; set; } = default!;
        [BindProperty]
        public List<RolePermissionVM> Permissions { get; set; } = new();
        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
            {
                return NotFound();
            }
            Role = role;

            var allPermissions = await _applicationDbContext.Permissions.Where(p => p.IsEnabled).ToListAsync();
            var roleClaims = await _roleManager.GetClaimsAsync(role) ?? new List<Claim>();
            Permissions = allPermissions.Select(p => new RolePermissionVM
            {
                PermissionDisplayName = p.DisplayName,
                PermissionName = p.Name,
                Selected = roleClaims.Any(c => c.Type == "Permission" && c.Value == p.Name)
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
            {
                return NotFound();
            }

            Role = role;
            // 取到页面勾选的权限
            var selectedPermissionNames = Permissions
                .Where(p => p.Selected)
                .Select(p => p.PermissionName)
                .ToList();

            // 当前角色已有的 Permission Claim
            var roleClaims = await _roleManager.GetClaimsAsync(Role) ?? new List<Claim>();

            // 删除未勾选的
            foreach (var claim in roleClaims.Where(c => c.Type == "Permission"))
            {
                if (!selectedPermissionNames.Contains(claim.Value))
                {
                    await _roleManager.RemoveClaimAsync(Role, claim);
                }
            }

            // 添加新勾选的
            foreach (var permName in selectedPermissionNames)
            {
                if (!roleClaims.Any(c => c.Type == "Permission" && c.Value == permName))
                {
                    await _roleManager.AddClaimAsync(Role, new Claim("Permission", permName));
                }
            }


            return RedirectToPage("./Index");
        }
    }
}
