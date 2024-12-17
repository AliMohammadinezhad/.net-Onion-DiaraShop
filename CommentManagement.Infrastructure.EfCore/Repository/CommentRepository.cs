using CommentManagement.Contract.Comment;
using CommentManagement.Domain.CommentAgg;
using Framework.Application;
using Framework.Infrastructure;

namespace CommentManagement.Infrastructure.EfCore.Repository;

public class CommentRepository : RepositoryBase<long, Comment>, ICommentRepository
{
    private readonly CommentContext _context;
    public CommentRepository(CommentContext context) : base(context)
    {
        _context = context;
    }


    public List<CommentViewModel> Search(CommentSearchModel searchModel)
    {
        var query = _context.Comments
            .Select(x => new CommentViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Website = x.Website,
                Message = x.Message,
                IsCancelled = x.IsCancelled,
                IsConfirmed = x.IsConfirmed,
                OwnerRecordId = x.OwnerRecordId,
                CommentDate = x.CreationDate.ToFarsi()
            });

        if (!string.IsNullOrWhiteSpace(searchModel.Name))
            query = query.Where(x => x.Name.Contains(searchModel.Name));

        if (!string.IsNullOrWhiteSpace(searchModel.Email))
            query = query.Where(x => x.Email.Contains(searchModel.Email));

        return query.OrderByDescending(x => x.Id).ToList();
    }
}