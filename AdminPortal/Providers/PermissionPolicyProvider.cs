using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AdminPortal.Providers
{
    public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
        }

        public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var mark = "Permission:";
            if (policyName.StartsWith(mark))
            {
                var permission = policyName.Substring(mark.Length);
                var policy = new AuthorizationPolicyBuilder().AddRequirements(new PermissionRequirement(permission)).Build();

                return Task.FromResult<AuthorizationPolicy?>(policy);
            }
            return base.GetPolicyAsync(policyName);
        }
    }
}
