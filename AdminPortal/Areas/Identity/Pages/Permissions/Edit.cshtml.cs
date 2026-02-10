using AdminPortal.Data;
using AdminPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Areas.Identity.Pages.Permissions
{
    [Authorize(Policy = "Permission:Permission.Edit")]
    public class EditModel : PageModel
    {
        private readonly AdminPortal.Data.ApplicationDbContext _context;

        public EditModel(AdminPortal.Data.ApplicationDbContext context)
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
            if (permission == null)
            {
                return NotFound();
            }
            Permission = permission;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Permission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermissionExists(Permission.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PermissionExists(int id)
        {
            return _context.Permissions.Any(e => e.Id == id);
        }
    }
}
