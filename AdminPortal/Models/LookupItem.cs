// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace AdminPortal.Models
{
    public class LookupItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = default!;   // 显示名称

        [Required]
        [MaxLength(100)]
        public string Code { get; set; } = default!;   // 唯一编码（程序使用）

        [MaxLength(200)]
        public string? Description { get; set; }       // 描述

        public int? ParentId { get; set; }
        public LookupItem? Parent { get; set; }

        public ICollection<LookupItem> Children { get; set; } = new List<LookupItem>();

        public int Level { get; set; }                 // 层级深度

        public int Sort { get; set; }                  // 排序

        [MaxLength(500)]
        public string Path { get; set; } = default!;   // 树路径 /1/5/12/

        public string Type { get; set; } = default!;   // 分类类型（关键字段）

        public bool IsEnabled { get; set; } = true;    // 是否启用

        public bool IsSystem { get; set; } = false;    // 是否系统内置

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
