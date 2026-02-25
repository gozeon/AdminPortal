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
    public class IndexModel : PageModel
    {
        private readonly AdminPortal.Data.ApplicationDbContext _context;

        public IndexModel(AdminPortal.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<LookupItem> LookupItem { get; set; } = default!;

        public async Task OnGetAsync()
        {
            LookupItem = await _context.LookupItems
                .Include(l => l.Parent).ToListAsync();
        }
    }
}
