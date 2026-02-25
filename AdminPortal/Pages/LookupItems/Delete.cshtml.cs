using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Data;
using AdminPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Pages.LookupItems
{
    public class DeleteModel : PageModel
    {
        private readonly AdminPortal.Data.ApplicationDbContext _context;

        public DeleteModel(AdminPortal.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LookupItem LookupItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lookupitem = await _context.LookupItems.FirstOrDefaultAsync(m => m.Id == id);

            if (lookupitem is not null)
            {
                LookupItem = lookupitem;

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

            var lookupitem = await _context.LookupItems.FindAsync(id);
            if (lookupitem != null)
            {
                LookupItem = lookupitem;
                _context.LookupItems.Remove(LookupItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
