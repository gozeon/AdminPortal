// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AdminPortal.Models;

namespace AdminPortal.Services.FilePreview
{
    public class PDFFilePreviewStrategy : IFilePreviewStrategy
    {
        private static readonly string[] SupportedExtensions = { ".pdf" };
        private static readonly string[] SupportedContentTypes = { "application/pdf" };

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
                PreviewType = FilePreviewType.PDF,
                Metadata = new Dictionary<string, string>
                {
                    { "fileName", appFile.OriginalName },
                    { "format", "PDF" }
                }
            };

            return Task.FromResult(result);
        }

        public FilePreviewType GetPreviewType() => FilePreviewType.PDF;
    }
}
