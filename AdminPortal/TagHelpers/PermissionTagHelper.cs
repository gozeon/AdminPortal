using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AdminPortal.TagHelpers
{
    [HtmlTargetElement(Attributes = "asp-permission")]
    public class PermissionTagHelper : TagHelper
    {
        private readonly IAuthorizationService _authorizationService;

        public PermissionTagHelper(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HtmlAttributeName("asp-permission")]
        public string? PermissionPolicy { get; set; } = default!;
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = null!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(PermissionPolicy))
            {
                return;
            }

            var user = ViewContext.HttpContext.User;
            var ok = (await _authorizationService.AuthorizeAsync(user, PermissionPolicy)).Succeeded;

            if (!ok)
            {
                // 如果验证失败，不渲染该 HTML 标签及其内部的所有内容
                output.SuppressOutput();
            }
        }
    }
}
