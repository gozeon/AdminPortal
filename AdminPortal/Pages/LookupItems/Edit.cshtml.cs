using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Data;
using AdminPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Pages.LookupItems
{
    public class EditModel : PageModel
    {
        private readonly AdminPortal.Data.ApplicationDbContext _context;

        public EditModel(AdminPortal.Data.ApplicationDbContext context)
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
            if (lookupitem == null)
            {
                return NotFound();
            }
            LookupItem = lookupitem;
            ViewData["ParentId"] = new SelectList(_context.LookupItems, "Id", "Code");
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

            _context.Attach(LookupItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LookupItemExists(LookupItem.Id))
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

        private bool LookupItemExists(int id)
        {
            return _context.LookupItems.Any(e => e.Id == id);
        }
    }
}
