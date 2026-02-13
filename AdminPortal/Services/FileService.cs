// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography;
using AdminPortal.Data;
using AdminPortal.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace AdminPortal.Services
{
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IFileStorage _fileStorage;
        public FileService(ApplicationDbContext applicationDbContext, IFileStorage fileStorage)
        {
            _applicationDbContext = applicationDbContext;
            _fileStorage = fileStorage;
        }
        public Task DeleteAsync(Guid id)
        {
            // 禁止删除
            throw new NotImplementedException();
        }
        public async Task<List<AppFile>> GetListAsync()
        {
            return await _applicationDbContext.AppFiles.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        public async Task<IPagedList<AppFile>> GetPagedListAsync(PagedRequest request)
        {
            return await _applicationDbContext.AppFiles.OrderByDescending(x => x.CreatedAt).ToPagedListAsync(request.PageNumber, request.PageSize);
}

        public async Task<AppFile> UploadAsync(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var hash = await ComputeHashAsync(stream);
                var existing = await _applicationDbContext.AppFiles.FirstOrDefaultAsync(x => x.Hash == hash);
                if(existing is not null)
                {
                    return existing;
                }

                stream.Position = 0;
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var storagePath = await _fileStorage.SaveAsync(stream, fileName);

                var entity = new AppFile
                {
                    FileName = fileName,
                    OriginalName = file.FileName,
                    ContentType = file.ContentType,
                    Size = file.Length,
                    Hash = hash,
                    StoragePath = storagePath,
                    Url = _fileStorage.GetUrl(storagePath)
                };

                _applicationDbContext.Add(entity);
                await _applicationDbContext.SaveChangesAsync();

                return entity;
            }
        }

        private async Task<string> ComputeHashAsync(Stream stream)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = await sha256.ComputeHashAsync(stream);
            return Convert.ToHexString(hashBytes);
        }
    }
}
