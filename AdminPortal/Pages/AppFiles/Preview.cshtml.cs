using AdminPortal.Data;
using AdminPortal.Models;
using AdminPortal.Services.FilePreview;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Pages.AppFiles
{
    [Authorize(Policy = "Permission:AppFile.Read")]
    public class PreviewModel : PageModel
    {
        private readonly IFilePreviewService _filePreviewService;
        private readonly ApplicationDbContext _applicationDbContext;

        public PreviewModel(IFilePreviewService filePreviewService, ApplicationDbContext applicationDbContext)
        {
            _filePreviewService = filePreviewService;
            _applicationDbContext = applicationDbContext;
        }

        public AppFile AppFile { get; set; } = default!;
        public FilePreviewResult PreviewResult { get; set; } = default!;

        public async Task OnGet(int id)
        {
            AppFile = await _applicationDbContext.AppFiles.FirstOrDefaultAsync(x => x.Id == id);
            if (AppFile is null)
            {
                PreviewResult = new FilePreviewResult
                {
                    Success = false,
                    ErrorMessage = "文件不存在"
                };
                return;
            }

            PreviewResult = await _filePreviewService.PreviewAsync(AppFile);
        }
    }
}
