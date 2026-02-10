using AdminPortal.Models;
using AdminPortal.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace AdminPortal.Areas.Identity.Pages.User
{
    [Authorize(Policy = "Permission:User.Add")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AdminOption _adminOption;
        public CreateModel(UserManager<IdentityUser> userManager, IOptions<AdminOption> adminOption)
        {
            _userManager = userManager;
            _adminOption = adminOption.Value;
        }
        [BindProperty]
        public UserVM UserVM { get; set; } = new();
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine(ModelState.IsValid);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new IdentityUser
            {
                UserName = UserVM.Email,
                Email = UserVM.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, _adminOption.AdminPassword);
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
