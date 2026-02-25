// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AdminPortal.Data;
using AdminPortal.Models;
using AdminPortal.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Services.FilePreview
{
    /// <summary>
    /// 文件预览服务
    /// </summary>
    public class FilePreviewService : IFilePreviewService
    {
        private readonly IFilePreviewFactory _factory;
        private readonly IFileStorage _fileStorage;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<FilePreviewService> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FilePreviewService(
            IFilePreviewFactory factory,
            IFileStorage fileStorage,
            ApplicationDbContext dbContext,
            ILogger<FilePreviewService> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _factory = factory;
            _fileStorage = fileStorage;
            _dbContext = dbContext;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<FilePreviewResult> PreviewAsync(int fileId)
        {
            try
            {
                var appFile = await _dbContext.AppFiles.FirstOrDefaultAsync(x => x.Id == fileId);

                if (appFile == null)
                {
                    return new FilePreviewResult
                    {
                        Success = false,
                        ErrorMessage = "文件不存在",
                        PreviewType = FilePreviewType.Unsupported
                    };
                }

                return await PreviewAsync(appFile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"预览文件失败: FileId={fileId}");
                return new FilePreviewResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    PreviewType = FilePreviewType.Unsupported
                };
            }
        }

        public async Task<FilePreviewResult> PreviewAsync(AppFile appFile)
        {
            try
            {
                var fileExtension = Path.GetExtension(appFile.FileName);
                var strategy = _factory.GetPreviewStrategy(appFile.ContentType, fileExtension);

                if (strategy == null)
                {
                    return new FilePreviewResult
                    {
                        Success = false,
                        ErrorMessage = $"不支持预览此文件类型: {appFile.OriginalName} / {appFile.ContentType}",
                        PreviewType = FilePreviewType.Unsupported,
                        PreviewUrl = appFile.Url
                    };
                }

                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, appFile.StoragePath);
                var result = await strategy.GeneratePreviewAsync(appFile, filePath);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"预览文件失败: FileName={appFile.FileName}");
                return new FilePreviewResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    PreviewType = FilePreviewType.Unsupported,
                    PreviewUrl = appFile.Url
                };
            }
        }

        public bool CanPreview(string contentType, string fileExtension)
        {
            var strategy = _factory.GetPreviewStrategy(contentType, fileExtension);
            return strategy != null;
        }

        public IEnumerable<FilePreviewType> GetSupportedPreviewTypes()
        {
            return _factory.GetSupportedPreviewTypes();
        }
    }

    public interface IFilePreviewService
    {
        Task<FilePreviewResult> PreviewAsync(int fileId);
        Task<FilePreviewResult> PreviewAsync(AppFile appFile);
        bool CanPreview(string contentType, string fileExtension);
        IEnumerable<FilePreviewType> GetSupportedPreviewTypes();
    }
}
