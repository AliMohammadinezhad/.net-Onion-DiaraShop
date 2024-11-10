using Framework.Application;
using Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Contracts.ProductPicture;
using ShopManagement.Domain.ProductPictureAgg;

namespace ShopManagement.Infrastructure.EfCore.Repository;

public class ProductPictureRepository : RepositoryBase<long, ProductPicture>, IProductPictureRepository
{
    private readonly ApplicationDbContext _context;

    public ProductPictureRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public EditProductPicture GetDetails(long id)
    {
        return _context.ProductPictures.Select(x => new EditProductPicture
        {
            Id = x.Id,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            ProductId = x.ProductId
        }).FirstOrDefault(x => x.Id == id);
    }

    public ProductPicture GetWithProductAndCategoryById(long id)
    {
        return _context.ProductPictures
            .Include(x => x.Product)
            .ThenInclude(x => x.Category)
            .FirstOrDefault(x => x.Id == id);
    }

    public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel)
    {
        var query = _context.ProductPictures.Include(x => x.Product).Select(x => new ProductPictureViewModel
        {
            //Picture = x.Picture,
            CreationDate = x.CreationDate.ToFarsi(),
            Id = x.Id,
            Product = x.Product.Name,
            ProductId = x.ProductId,
            IsRemoved = x.IsRemoved
        });

        if (searchModel.ProductId != 0)
            query = query.Where(x => x.ProductId == searchModel.ProductId);

        return query.OrderByDescending(x => x.Id).ToList();
    }
}