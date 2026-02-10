using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace AdminPortal.Areas.Identity.Pages.Role
{
    [Authorize(Policy = "Permission:Role.Read")]
    public class IndexModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public IndexModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IList<IdentityRole> Roles { get; set; } = default!;

        public void OnGet()
        {
            Roles = _roleManager.Roles.ToList();
        }
    }
}
