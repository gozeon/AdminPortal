// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;

namespace AdminPortal.Services.FileStorage
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        // TXT 文件预览的最大字节数（默认 1MB）
        private const long MaxPreviewFileSize = 1024 * 1024;

        public LocalFileStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public Task DeleteAsync(string path)
        {
            // 本地禁止删除
            throw new NotImplementedException();
        }
        public string GetUrl(string path)
        {
            return "/" + path;
        }

        public async Task<string> ReadAsStringAsync(string storagePath)
        {
            try
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, storagePath);

                // 安全检查：防止目录遍历攻击
                var fullPath = Path.GetFullPath(filePath);
                var allowedPath = Path.GetFullPath(_webHostEnvironment.WebRootPath);

                if (!fullPath.StartsWith(allowedPath, StringComparison.OrdinalIgnoreCase))
                {
                    throw new UnauthorizedAccessException("访问路径不合法");
                }

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"文件不存在: {storagePath}");
                }

                var fileInfo = new FileInfo(filePath);

                // 检查文件大小，防止加载过大文件
                if (fileInfo.Length > MaxPreviewFileSize)
                {
                    throw new InvalidOperationException($"文件过大，预览限制为 {MaxPreviewFileSize / 1024 / 1024}MB");
                }

                // 读取文件内容，自动检测编码
                using (var sr = new StreamReader(filePath, Encoding.UTF8, detectEncodingFromByteOrderMarks: true))
                {
                    return await sr.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"读取文件失败: {ex.Message}", ex);
            }
        }

        public async Task<string> SaveAsync(Stream stream, string fileName)
        {
            var folderName = "uploads";
            var folder = Path.Combine(_webHostEnvironment.WebRootPath, folderName);
            Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, fileName);
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(fs);
            }

            return $"{folderName}/{fileName}";
        }
    }
}
