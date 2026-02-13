using System;

namespace AdminPortal.Attributes;

public class AuthorizeHandlerAttribute : Attribute
{
    public string Policy { get; }
    public AuthorizeHandlerAttribute(string policy)
    {
        Policy = policy;
    }
}
