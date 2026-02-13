using AdminPortal.Models;
using AdminPortal.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Data;
using System.Security.Claims;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace AdminPortal.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            try
            {
                var options = serviceProvider.GetRequiredService<IOptions<AdminOption>>().Value;
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var db = serviceProvider.GetRequiredService<ApplicationDbContext>();


                await SeedRole(roleManager, options);
                await SeedPermission(roleManager, db, options);
                await SeedAdmin(roleManager, userManager, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private static async Task SeedPermission(RoleManager<IdentityRole> roleManager, ApplicationDbContext db, AdminOption options)
        {
            // 插入默认权限
            if (db.Permissions.Any()) return;

            var defaultPermissions = new List<Permission>
            {
                new Permission { Name = "User.Read", Group = "User", DisplayName = "查看用户" },
                new Permission { Name = "User.Delete", Group = "User", DisplayName = "删除用户" },
                new Permission { Name = "User.Edit", Group = "User", DisplayName = "修改用户" },
                new Permission { Name = "User.Add", Group = "User", DisplayName = "增加用户" },

                new Permission { Name = "Role.Read", Group = "Role", DisplayName = "查看角色" },
                new Permission { Name = "Role.Delete", Group = "Role", DisplayName = "删除角色" },
                new Permission { Name = "Role.Edit", Group = "Role", DisplayName = "修改角色" },
                new Permission { Name = "Role.Add", Group = "Role", DisplayName = "增加角色" },

                new Permission { Name = "Permission.Read", Group = "Permission", DisplayName = "查看权限" },
                new Permission { Name = "Permission.Delete", Group = "Permission", DisplayName = "删除权限" },
                new Permission { Name = "Permission.Edit", Group = "Permission", DisplayName = "修改权限" },
                new Permission { Name = "Permission.Add", Group = "Permission", DisplayName = "增加权限" },

                new Permission { Name = "AppFile.Read", Group = "AppFile", DisplayName = "查看文件" },
                new Permission { Name = "AppFile.Add", Group = "AppFile", DisplayName = "增加文件" }
            };

            db.Permissions.AddRange(defaultPermissions);

            await db.SaveChangesAsync();

            // admin 角色插入权限，admin已经是最高权限了，所有
            //var adminRole = await roleManager.FindByNameAsync(options.AdminRoleName);
            //if (adminRole == null) return;

            //var adminClaims = await roleManager.GetClaimsAsync(adminRole);
            //foreach (var permission in defaultPermissions)
            //{
            //    await roleManager.AddClaimAsync(adminRole, new Claim("Permission", permission.Name));
            //}
        }

        private static async Task SeedAdmin(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, AdminOption options)
        {
            var user = await userManager.FindByEmailAsync(options.AdminEmail);
            if (user is null)
            {
                user = new IdentityUser
                {
                    UserName = options.AdminEmail,
                    Email = options.AdminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, options.AdminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));
                }
            }
            await userManager.AddToRoleAsync(user, options.AdminRoleName);
        }

        private static async Task SeedRole(RoleManager<IdentityRole> roleManager, AdminOption options)
        {
            string[] roles = new[] { options.AdminRoleName, };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
