using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Data;
using AdminPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminPortal.Areas.Identity.Pages.Permissions
{
    [Authorize(Policy = "Permission:Permission.Add")]
    public class CreateModel : PageModel
    {
        private readonly AdminPortal.Data.ApplicationDbContext _context;

        public CreateModel(AdminPortal.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Permission Permission { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Permissions.Add(Permission);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
