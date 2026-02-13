// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace AdminPortal.Models
{
    public class AppFile
    {
        public int Id { get; set; }
        public string FileName { get; set; } = default!;
        public string OriginalName { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public long Size { get; set; }

        public string Hash { get; set; } = default!;
        public string StoragePath { get; set; } = default!;
        public string Url { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
