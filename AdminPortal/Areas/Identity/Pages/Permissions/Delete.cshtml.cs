using AdminPortal.Data;
using AdminPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Areas.Identity.Pages.Permissions
{
    [Authorize(Policy = "Permission:Permission.Delete")]
    public class DeleteModel : PageModel
    {
        private readonly AdminPortal.Data.ApplicationDbContext _context;

        public DeleteModel(AdminPortal.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Permission Permission { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.Permissions.FirstOrDefaultAsync(m => m.Id == id);

            if (permission is not null)
            {
                Permission = permission;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.Permissions.FindAsync(id);
            if (permission != null)
            {
                Permission = permission;
                _context.Permissions.Remove(Permission);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
