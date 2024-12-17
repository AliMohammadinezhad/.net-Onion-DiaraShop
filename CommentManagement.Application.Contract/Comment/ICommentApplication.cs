using Framework.Application;

namespace CommentManagement.Contract.Comment;

public interface ICommentApplication
{
    OperationResult Add(AddComment command);
    OperationResult Confirm(long id);
    OperationResult Cancel(long id);
    List<CommentViewModel> Search(CommentSearchModel searchModel);
}