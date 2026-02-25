using AdminPortal.Attributes;
using AdminPortal.Models;
using AdminPortal.Services.FileStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace AdminPortal.Pages.AppFiles
{
    [Authorize(Policy = "Permission:AppFile.Read")]
    public class IndexModel : PageModel
    {
        private readonly IFileService _fileService;
        public IndexModel(IFileService fileService)
        {
            _fileService = fileService;
        }

        [BindProperty(SupportsGet = true)]
        public PagedRequest Query { get; set; } = new();

        [BindProperty]
        public List<IFormFile>? UploadFile { get; set; }

        public IPagedList<AppFile> AppFilesPagedList { get; set; } = default!;

        public async Task OnGetAsync()
        {
            AppFilesPagedList = await _fileService.GetPagedListAsync(Query);
        }

        [AuthorizeHandler("Permission:AppFile.Add")]
        public async Task<IActionResult> OnPostAsync()
        {
            var maxFilesCount = 10;
            if (UploadFile == null || !UploadFile.Any() || UploadFile.Count > 10)
            {
                ModelState.AddModelError("", $"请选择文件，仅支持同时上传{maxFilesCount}个文件");
                await OnGetAsync();
                return Page();
            }

            foreach (var item in UploadFile)
            {
                if (item.Length == 0)
                {
                    continue;
                }
                await _fileService.UploadAsync(item);
            }

            return RedirectToPage();
        }
    }
}
