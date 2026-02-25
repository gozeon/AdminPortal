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
    public class DetailsModel : PageModel
    {
        private readonly AdminPortal.Data.ApplicationDbContext _context;

        public DetailsModel(AdminPortal.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
