// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AdminPortal.Models;

namespace AdminPortal.Services
{
    /// <summary>
    /// 文件预览策略接口
    /// </summary>
    public interface IFilePreviewStrategy
    {
        /// <summary>
        /// 检查是否支持该文件类型
        /// </summary>
        bool CanPreview(string contentType, string fileExtension);

        /// <summary>
        /// 生成预览内容
        /// </summary>
        Task<FilePreviewResult> GeneratePreviewAsync(AppFile appFile, string filePath);

        /// <summary>
        /// 获取预览类型
        /// </summary>
        FilePreviewType GetPreviewType();
    }

    /// <summary>
    /// 文件预览结果
    /// </summary>
    public class FilePreviewResult
    {
        public bool Success { get; set; }
        public string? PreviewUrl { get; set; }
        public FilePreviewType PreviewType { get; set; }
        public string? ErrorMessage { get; set; }
        public Dictionary<string, string>? Metadata { get; set; }
    }

    /// <summary>
    /// 预览类型枚举
    /// </summary>
    public enum FilePreviewType
    {
        /// <summary>
        /// 图片预览
        /// </summary>
        Image,
        /// <summary>
        /// PDF 预览
        /// </summary>
        PDF,

        /// <summary>
        /// 文本预览
        /// </summary>
        Text,

        /// <summary>
        /// 不支持预览
        /// </summary>
        Unsupported
    }
}
