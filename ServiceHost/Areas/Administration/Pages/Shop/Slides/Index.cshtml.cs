using Framework.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Contracts.Slide;

namespace ServiceHost.Areas.Administration.Pages.Shop.Slides;
    public class IndexModel : PageModel
    {
        [TempData] public string Message { get; set; }
        public List<SlideViewModel> Slides;

        private readonly ISlideApplication _slideApplication;

        public IndexModel(ISlideApplication slideApplication)
        {
            _slideApplication = slideApplication;
        }


        public void OnGet()
        {
            Slides = _slideApplication.GetList();
        }

        public IActionResult OnGetCreate()
        {
            var command = new CreateSlide();
            return Partial("./Create", command);
        }

        public IActionResult OnPostCreate(CreateSlide command)
        {
            var result = _slideApplication.Create(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var slide = _slideApplication.GetDetails(id);
            return Partial("./Edit", slide);
        }

        public JsonResult OnPostEdit(EditSlide command)
        {
            var result = _slideApplication.Edit(command);
            return new JsonResult(result);
        }

        public RedirectToPageResult OnGetRemoved(long id)
        {
            OperationResult result = _slideApplication.Remove(id);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");
            Message = result.Message;
            return RedirectToPage("./Index");
        }

        public RedirectToPageResult OnGetRestore(long id)
        {
            OperationResult result = _slideApplication.Restore(id);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");
            Message = result.Message;
            return RedirectToPage("./Index");
        }
    } 

