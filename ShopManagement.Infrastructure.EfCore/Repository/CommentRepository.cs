using Framework.Application;
using Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Contracts.Comment;
using ShopManagement.Domain.CommentAgg;

namespace ShopManagement.Infrastructure.EfCore.Repository;

public class CommentRepository : RepositoryBase<long, Comment>, ICommentRepository
{
    private readonly ApplicationDbContext _context;
    public CommentRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }


    public List<CommentViewModel> Search(CommentSearchModel searchModel)
    {
        var query = _context.Comments
            .Include(x => x.Product)
            .Select(x => new CommentViewModel
        {
            Id = x.Id,
            Email = x.Email,
            IsCancelled = x.IsCancelled,
            IsConfirmed = x.IsConfirmed,
            Message = x.Message,
            Name = x.Name,
            ProductId = x.ProductId,
            ProductName = x.Product.Name,
            CommentDate = x.CreationDate.ToFarsi()
        });

        if (!string.IsNullOrWhiteSpace(searchModel.Name))
            query = query.Where(x => x.Name.Contains(searchModel.Name));
        
        if (!string.IsNullOrWhiteSpace(searchModel.Email))
            query = query.Where(x => x.Email.Contains(searchModel.Email));
        
        return query.OrderByDescending(x => x.Id).ToList();
    }
}