// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace AdminPortal.Services
{
    /// <summary>
    /// 文件预览工厂 (Factory Pattern)
    /// </summary>
    public class FilePreviewFactory : IFilePreviewFactory
    {
        private readonly IEnumerable<IFilePreviewStrategy> _strategies;
        private readonly ILogger<FilePreviewFactory> _logger;

        public FilePreviewFactory(IEnumerable<IFilePreviewStrategy> strategies, ILogger<FilePreviewFactory> logger)
        {
            _strategies = strategies;
            _logger = logger;
        }

        public IFilePreviewStrategy? GetPreviewStrategy(string contentType, string fileExtension)
        {
            try
            {
                var strategy = _strategies.FirstOrDefault(s => s.CanPreview(contentType, fileExtension));

                if (strategy == null)
                {
                    _logger.LogWarning($"未找到支持的预览策略: ContentType={contentType}, Extension={fileExtension}");
                }

                return strategy;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取预览策略失败");
                return null;
            }
        }

        public IEnumerable<FilePreviewType> GetSupportedPreviewTypes()
        {
            return _strategies.Select(s => s.GetPreviewType()).Distinct();
        }
    }

    public interface IFilePreviewFactory
    {
        IFilePreviewStrategy? GetPreviewStrategy(string contentType, string fileExtension);
        IEnumerable<FilePreviewType> GetSupportedPreviewTypes();
    }
}
