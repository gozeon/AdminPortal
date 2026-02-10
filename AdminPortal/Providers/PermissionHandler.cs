using AdminPortal.Data;
using AdminPortal.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AdminPortal.Providers
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly ApplicationDbContext _db;
        private readonly AdminOption _adminOption;

        public PermissionHandler(ApplicationDbContext db, IOptions<AdminOption> adminOption)
        {
            _db = db;
            _adminOption = adminOption.Value;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // admin 直接放行
            if (context.User.IsInRole(_adminOption.AdminRoleName))
            {
                context.Succeed(requirement);
                return;
            }

            // 从数据库查询权限表，因为权限有可能被禁用或删除
            var enabledPermissions = await _db.Permissions.Where(p => p.IsEnabled).Select(p => p.Name).ToListAsync();

            var hasPermission = context.User.Claims.Any(c => c.Type == "Permission" && c.Value == requirement.Permission && enabledPermissions.Contains(c.Value));
            if (hasPermission)
            {
                context.Succeed(requirement);
            }

            return;
        }
    }
}
