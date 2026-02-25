// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AdminPortal.Models;
using X.PagedList;

namespace AdminPortal.Services.FileStorage
{
    public interface IFileService
    {
        Task<AppFile> UploadAsync(IFormFile file);
        Task<List<AppFile>> GetListAsync();
        Task DeleteAsync(Guid id);
        Task<IPagedList<AppFile>> GetPagedListAsync(PagedRequest request);
    }
}
