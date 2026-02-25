using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Data;
using AdminPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Areas.Identity.Pages.Permissions
{
    [Authorize(Policy = "Permission:Permission.Read")]
    public class IndexModel : PageModel
    {
        private readonly AdminPortal.Data.ApplicationDbContext _context;

        public IndexModel(AdminPortal.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Permission> Permission { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Permission = await _context.Permissions.ToListAsync();
        }
    }
}
