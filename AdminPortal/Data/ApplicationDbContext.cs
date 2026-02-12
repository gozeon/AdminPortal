using AdminPortal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<LookupItem> LookupItems => Set<LookupItem>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Permission>().HasIndex(x => x.Name).IsUnique();

            builder.Entity<LookupItem>(b =>
            {
                b.HasKey(x => x.Id);

                b.HasOne(x => x.Parent)
                    .WithMany(x => x.Children)
                    .HasForeignKey(x => x.ParentId)
                    .OnDelete(DeleteBehavior.Restrict); // 禁止删除

                b.HasIndex(x => x.Type);
                // 同一 Type 下，Code 必须唯一
                b.HasIndex(x => new { x.Type, x.Code }).IsUnique();
                b.HasIndex(x => new { x.ParentId, x.Name });
            });

            base.OnModelCreating(builder);
        }
    }
}
