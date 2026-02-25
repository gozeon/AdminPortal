// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AdminPortal.Models;

namespace AdminPortal.Services
{
    /// <summary>
    /// 文本文件预览策略
    /// </summary>
    public class TextFilePreviewStrategy : IFilePreviewStrategy
    {
        private static readonly string[] SupportedExtensions =
         {
            ".txt", ".log", ".csv", ".json", ".xml", ".html",
            ".css", ".js", ".cs", ".md", ".sql", ".config"
        };

        private static readonly string[] SupportedContentTypes =
        {
            "text/plain",
            "text/csv",
            "application/json",
            "application/xml",
            "text/xml",
            "text/html",
            "text/css",
            "text/javascript",
            "application/x-csharp",
            "text/markdown",
            "application/x-sql"
        };

        private readonly IFileStorage _fileStorage;
        private readonly ILogger<TextFilePreviewStrategy> _logger;

        public TextFilePreviewStrategy(IFileStorage fileStorage, ILogger<TextFilePreviewStrategy> logger)
        {
            _fileStorage = fileStorage;
            _logger = logger;
        }

        public bool CanPreview(string contentType, string fileExtension)
        {
            // 检查扩展名
            if (SupportedExtensions.Contains(fileExtension.ToLower()))
            {
                return true;
            }

            // 检查 Content-Type
            if (SupportedContentTypes.Any(ct =>
                contentType.ToLower().Contains(ct.ToLower())))
            {
                return true;
            }

            // 对于 application/octet-stream，检查扩展名 二进制
            if (contentType.ToLower() == "application/octet-stream" &&
                SupportedExtensions.Contains(fileExtension.ToLower()))
            {
                return true;
            }

            return false;
        }

        public async Task<FilePreviewResult> GeneratePreviewAsync(AppFile appFile, string filePath)
        {
            try
            {
                // 从存储中读取文件内容
                var content = await _fileStorage.ReadAsStringAsync(appFile.StoragePath);

                // 限制显示长度（防止界面卡顿）
                var displayContent = content;
                var maxDisplayLength = 1024 * 50; // 50KB

                if (content.Length > maxDisplayLength)
                {
                    displayContent = content.Substring(0, maxDisplayLength) +
                        $"\n\n... (文件过大，已截断，总行数: {content.Split('\n').Length} 行)";
                }

                // 获取文件统计信息
                var lines = content.Split('\n', StringSplitOptions.None);
                var encoding = DetectEncoding(filePath);

                var result = new FilePreviewResult
                {
                    Success = true,
                    PreviewType = FilePreviewType.Text,
                    Metadata = new Dictionary<string, string>
                    {
                        { "fileName", appFile.OriginalName },
                        { "fileExtension", Path.GetExtension(appFile.OriginalName) },
                        { "lines", lines.Length.ToString() },
                        { "encoding", encoding },
                        { "content", displayContent }
                    }
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"读取文本文件失败: FileName={appFile.FileName}");
                return new FilePreviewResult
                {
                    Success = false,
                    ErrorMessage = $"文本文件预览失败: {ex.Message}",
                    PreviewType = FilePreviewType.Text
                };
            }
        }

        public FilePreviewType GetPreviewType() => FilePreviewType.Text;

        /// <summary>
        /// 检测文件编码
        /// </summary>
        private string DetectEncoding(string filePath)
        {
            try
            {
                using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var buffer = new byte[4];
                    file.ReadExactly(buffer, 0, 4);

                    // UTF-8 BOM
                    if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                        return "UTF-8 (with BOM)";

                    // UTF-16 LE BOM
                    if (buffer[0] == 0xff && buffer[1] == 0xfe)
                        return "UTF-16 LE (with BOM)";

                    // UTF-16 BE BOM
                    if (buffer[0] == 0xfe && buffer[1] == 0xff)
                        return "UTF-16 BE (with BOM)";

                    // UTF-32 BOM
                    if (buffer[0] == 0xff && buffer[1] == 0xfe && buffer[2] == 0x00 && buffer[3] == 0x00)
                        return "UTF-32 LE (with BOM)";

                    return "UTF-8";
                }
            }
            catch
            {
                return "Unknown";
            }
        }

    }
}
