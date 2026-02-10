using AdminPortal.Data;
using AdminPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AdminPortal.Areas.Identity.Pages.Role
{
    [Authorize(Policy = "Permission:Role.Read")]
    public class DetailModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;
        public DetailModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext)
        {
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
        }

        [BindProperty]
        public IdentityRole Role { get; set; } = default!;

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

            var roleClaims = await _roleManager.GetClaimsAsync(Role) ?? new List<Claim>();
            var permissionNames = roleClaims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList();

            var existingPermissions = await _applicationDbContext.Permissions.Where(p => permissionNames.Contains(p.Name)).ToListAsync();
            Permissions = existingPermissions.Select(p => new RolePermissionVM
            {
                PermissionDisplayName = p.DisplayName,
                PermissionName = p.Name,
                Selected = true
            }).ToList();
            return Page();
        }
    }
}
