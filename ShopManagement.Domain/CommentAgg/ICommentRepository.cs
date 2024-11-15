using Framework.Domain;
using ShopManagement.Contracts.Comment;

namespace ShopManagement.Domain.CommentAgg;

public interface ICommentRepository : IRepository<long, Comment>
{
    List<CommentViewModel> Search(CommentSearchModel  searchModel);
}