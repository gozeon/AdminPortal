using AdminPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Areas.Identity.Pages.User
{
    [Authorize(Policy = "Permission:User.Read")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        public IndexModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public List<UserVM> Users { get; set; } = new();
        public async Task<IActionResult> OnGet()
        {
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                Users.Add(new UserVM
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    Roles = roles.ToList()
                });
            }
            return Page();
        }
    }
}
