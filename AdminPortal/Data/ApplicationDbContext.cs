using AdminPortal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Permission> Permissions => Set<Permission>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Permission>().HasIndex(x => x.Name).IsUnique();

            base.OnModelCreating(builder);
        }
    }
}
