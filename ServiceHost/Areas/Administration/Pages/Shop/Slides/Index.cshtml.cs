using Framework.Application;
using Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Contracts.Slide;
using ShopManagement.infrastructure.Configuration.Permissions;

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

    [NeedsPermission(ShopPermissions.ListSlides)]
    public void OnGet()
    {
        Slides = _slideApplication.GetList();
    }

    [NeedsPermission(ShopPermissions.CreateSlides)]
    public IActionResult OnGetCreate()
    {
        var command = new CreateSlide();
        return Partial("./Create", command);
    }

    [NeedsPermission(ShopPermissions.CreateSlides)]
    public IActionResult OnPostCreate(CreateSlide command)
    {
        var result = _slideApplication.Create(command);
        return new JsonResult(result);
    }

    [NeedsPermission(ShopPermissions.EditSlides)]
    public IActionResult OnGetEdit(long id)
    {
        var slide = _slideApplication.GetDetails(id);
        return Partial("./Edit", slide);
    }

    [NeedsPermission(ShopPermissions.EditSlides)]
    public JsonResult OnPostEdit(EditSlide command)
    {
        var result = _slideApplication.Edit(command);
        return new JsonResult(result);
    }

    [NeedsPermission(ShopPermissions.RemoveSlides)]
    public RedirectToPageResult OnGetRemoved(long id)
    {
        OperationResult result = _slideApplication.Remove(id);
        if (result.IsSucceeded)
            return RedirectToPage("./Index");
        Message = result.Message;
        return RedirectToPage("./Index");
    }

    [NeedsPermission(ShopPermissions.RestoreSlides)]
    public RedirectToPageResult OnGetRestore(long id)
    {
        OperationResult result = _slideApplication.Restore(id);
        if (result.IsSucceeded)
            return RedirectToPage("./Index");
        Message = result.Message;
        return RedirectToPage("./Index");
    }
}

