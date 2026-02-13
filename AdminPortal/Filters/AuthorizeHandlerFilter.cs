using System;
using AdminPortal.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdminPortal.Filters;

public class AuthorizeHandlerFilter : IAsyncPageFilter
{
    private readonly IAuthorizationService _authorizationService;
    public AuthorizeHandlerFilter(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }
    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        var handler = context.HandlerMethod?.MethodInfo;
        var authAttr = handler?.GetCustomAttributes(typeof(AuthorizeHandlerAttribute), true).FirstOrDefault() as AuthorizeHandlerAttribute;

        if (authAttr != null)
        {
            var result = await _authorizationService.AuthorizeAsync(context.HttpContext.User, authAttr.Policy);
            if (!result.Succeeded)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
        await next();
    }

    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) => Task.CompletedTask;
}
