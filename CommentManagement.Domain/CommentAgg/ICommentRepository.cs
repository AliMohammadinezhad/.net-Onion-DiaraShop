using CommentManagement.Contract.Comment;
using Framework.Domain;

namespace CommentManagement.Domain.CommentAgg;

public interface ICommentRepository : IRepository<long, Comment>
{
    List<CommentViewModel> Search(CommentSearchModel searchModel);
}