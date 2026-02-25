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
    public class DetailsModel : PageModel
    {
        private readonly AdminPortal.Data.ApplicationDbContext _context;

        public DetailsModel(AdminPortal.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
