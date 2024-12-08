using CommentManagement.Contract.Comment;
using DiscountManagement.infrastructure.Configuration.Permissions;
using Framework.Application;
using Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Contracts.Slide;

namespace ServiceHost.Areas.Administration.Pages.Comments;
public class IndexModel : PageModel
{
    [TempData] public string Message { get; set; }

    public List<CommentViewModel> Comments;
    public CommentSearchModel SearchModel;
    private readonly ICommentApplication _commentApplication;

    public IndexModel(ICommentApplication commentApplication)
    {
        _commentApplication = commentApplication;
    }

    [NeedsPermission(CommentPermissions.ListComments)]
    public void OnGet(CommentSearchModel searchModel)
    {
        Comments = _commentApplication.Search(searchModel);
    }

    [NeedsPermission(CommentPermissions.CreateComment)]
    public IActionResult OnGetCreate()
    {
        var command = new CreateSlide();
        return Partial("./Create", command);
    }



    [NeedsPermission(CommentPermissions.ConfirmComment)]
    public RedirectToPageResult OnGetConfirm(long id)
    {
        OperationResult result = _commentApplication.Confirm(id);
        if (result.IsSucceeded)
            return RedirectToPage("./Index");
        Message = result.Message;
        return RedirectToPage("./Index");
    }

    [NeedsPermission(CommentPermissions.CancelComment)]
    public RedirectToPageResult OnGetCancel(long id)
    {
        OperationResult result = _commentApplication.Cancel(id);
        if (result.IsSucceeded)
            return RedirectToPage("./Index");
        Message = result.Message;
        return RedirectToPage("./Index");
    }
}

