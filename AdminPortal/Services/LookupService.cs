// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AdminPortal.Data;
using AdminPortal.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AdminPortal.Services
{
    public class LookupService : ILookupService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMemoryCache _cache;
       
        public LookupService(ApplicationDbContext applicationDbContext, IMemoryCache memoryCache)
        {
            _applicationDbContext = applicationDbContext;
            _cache = memoryCache;
        }

        private string CacheKey(string group) => $"lookup_c_{group}";

        public Task ClearCacheAsync(string? group = null)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                // TODO: 清除所有lookup的缓存
                return Task.CompletedTask;
            }
            _cache.Remove(CacheKey(group));
            return Task.CompletedTask;
        }
        public async Task<List<LookupItem>> GetByGroupAsync(string group)
        {
            return await _cache.GetOrCreateAsync(
                CacheKey(group),
                async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return await _applicationDbContext.LookupItems
                    .Where(x => x.Type == group && x.IsEnabled)
                    .OrderBy(x => x.Sort)
                    .AsNoTracking()
                    .ToListAsync();
                }
            ) ?? new List<LookupItem>();
        }
        public async Task<List<SelectListItem>> GetSelectListAsync(string group, string? selectedValue = null)
        {
            var list = await GetByGroupAsync(group);
            return list.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = selectedValue != null && x.Id.ToString() == selectedValue
            }).ToList();
        }
        public async Task<List<LookupTreeDto>> GetTreeAsync(string group)
        {
            var items = await GetByGroupAsync(group);
            var lookup = items.ToDictionary(
                x => x.Id,
                x => new LookupTreeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                });

            var roots = new List<LookupTreeDto>();
            foreach(var item in items)
            {
                if(item.ParentId.HasValue && lookup.ContainsKey(item.ParentId.Value))
                {
                    lookup[item.ParentId.Value].Children.Add(lookup[item.Id]);
                }
                else
                {
                    roots.Add(lookup[item.Id]);
                }
            }
            return roots;
        }

        public async Task<List<LookupTreeDto>> GetTreeByPathAsync(string group)
        {
            var items = await _applicationDbContext.LookupItems
            .Where(x => x.Type == group && x.IsEnabled)
            .OrderBy(x => x.Path)
            .AsNoTracking()
            .ToListAsync();

            var dict = new Dictionary<string, LookupTreeDto>();
            var roots = new List<LookupTreeDto>();

            foreach (var item in items)
            {
                var node = new LookupTreeDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Code = item.Code
                };

                dict[item.Path] = node;

                var lastDot = item.Path.LastIndexOf('.');

                if (lastDot == -1)
                {
                    // 根节点
                    roots.Add(node);
                }
                else
                {
                    var parentPath = item.Path.Substring(0, lastDot);

                    if (dict.TryGetValue(parentPath, out var parent))
                    {
                        parent.Children.Add(node);
                    }
                }
            }

            return roots;
        }

        public async Task<List<LookupItem>> GetSubTreeByPathAsync(string path)
        {
            return await _applicationDbContext.LookupItems.Where(x => x.Path.StartsWith(path)).OrderBy(x => x.Path).AsNoTracking().ToListAsync();
        }
    }
}
