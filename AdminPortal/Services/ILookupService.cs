// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AdminPortal.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminPortal.Services
{
    public interface ILookupService
    {
        Task<List<LookupItem>> GetByGroupAsync(string group);
        Task<List<LookupTreeDto>> GetTreeAsync(string group);
        Task<List<LookupTreeDto>> GetTreeByPathAsync(string group);
        Task<List<LookupItem>> GetSubTreeByPathAsync(string path);

        Task<List<SelectListItem>> GetSelectListAsync(string group, string? selectedValue = null);
        Task ClearCacheAsync(string? group = null);
    }
}
