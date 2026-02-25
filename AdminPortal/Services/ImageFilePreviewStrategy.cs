// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AdminPortal.Models;

namespace AdminPortal.Services
{
    public class ImageFilePreviewStrategy : IFilePreviewStrategy
    {
        private static readonly string[] SupportedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp", ".svg" };
        private static readonly string[] SupportedContentTypes = { "image/jpeg", "image/png", "image/gif", "image/webp", "image/bmp", "image/svg+xml" };

        public bool CanPreview(string contentType, string fileExtension)
        {
            return SupportedContentTypes.Contains(contentType.ToLower()) ||
                   SupportedExtensions.Contains(fileExtension.ToLower());
        }

        public Task<FilePreviewResult> GeneratePreviewAsync(AppFile appFile, string filePath)
        {
            var result = new FilePreviewResult
            {
                Success = true,
                PreviewUrl = appFile.Url,
                PreviewType = FilePreviewType.Image,
                Metadata = new Dictionary<string, string>
                {
                    { "fileName", appFile.OriginalName },
                    { "size", appFile.Size.ToString() }
                }
            };

            return Task.FromResult(result);
        }

        public FilePreviewType GetPreviewType() => FilePreviewType.Image;
    }
}
