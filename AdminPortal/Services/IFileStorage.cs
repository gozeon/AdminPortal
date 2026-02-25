// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace AdminPortal.Services
{
    public interface IFileStorage
    {
        Task<string> SaveAsync(Stream stream, string fileName);
        Task DeleteAsync(string path);
        string GetUrl(string path);

        /// <summary>
        /// 读取文件内容为字符串（用于文本文件预览）
        /// </summary>
        Task<string> ReadAsStringAsync(string storagePath);
    }
}
