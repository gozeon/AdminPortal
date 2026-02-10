using AdminPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPortal.Areas.Identity.Pages.User
{
    [Authorize(Policy = "Permission:User.Delete")]
    public class DeleteModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        public DeleteModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public UserVM UserVM { get; set; } = new();
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


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user is not null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToPage("./Index");
        }
    }
}
