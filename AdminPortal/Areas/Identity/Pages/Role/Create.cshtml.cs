using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace AdminPortal.Areas.Identity.Pages.Role
{
    [Authorize(Policy = "Permission:Role.Add")]
    public class CreateModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public CreateModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public void OnGet()
        {
        }

        [BindProperty]
        public IdentityRole Role { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _roleManager.CreateAsync(Role);
            if (result.Succeeded)
            {
                return RedirectToPage("./Index");

            }
            else
            {
                foreach (var item in result.Errors.Select(e => e.Description))
                {
                    ModelState.AddModelError("", item);
                }
                return Page();
            }

        }
    }
}
