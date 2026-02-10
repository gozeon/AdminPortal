using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace AdminPortal.Areas.Identity.Pages.Role
{
    [Authorize(Policy = "Permission:Role.Delete")]
    public class DeleteModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public DeleteModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public IdentityRole Role { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role is not null)
            {
                Role = role;
                return Page();
            }
            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role is not null)
            {
                Role = role;
                await _roleManager.DeleteAsync(role);
            }

            return RedirectToPage("./Index");
        }
    }
}
