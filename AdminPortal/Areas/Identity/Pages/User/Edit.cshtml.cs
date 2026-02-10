using AdminPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace AdminPortal.Areas.Identity.Pages.User
{
    [Authorize(Policy = "Permission:User.Edit")]
    public class EditModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public EditModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public UserVM UserVM { get; set; } = new();
        [BindProperty]
        public List<string> SelectRoles { get; set; } = new();
        public IList<IdentityRole> Roles { get; set; } = default!;
        public SelectList RoleOptions { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            UserVM = new UserVM
            {
                Id = id,
                Email = user.Email ?? "",
                Roles = roles.ToList()
            };

            SelectRoles = roles.ToList();
            RoleOptions = new SelectList(_roleManager.Roles.ToList(), nameof(IdentityRole.Name), nameof(IdentityRole.Name));

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            await _userManager.AddToRolesAsync(user, SelectRoles);

            return RedirectToPage("./Index");
        }
    }
}
