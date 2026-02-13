// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace AdminPortal.Services
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
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
