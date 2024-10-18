using Framework.Application;
using ShopManagement.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Application;

public class ProductCategoryApplication : IProductCategoryApplication
{

    private readonly IProductCategoryRepository _repository;

    public ProductCategoryApplication(IProductCategoryRepository repository)
    {
        _repository = repository;
    }

    public OperationResult Create(CreateProductCategory command)
    {
        var operation = new OperationResult();
        if (_repository.Exists(operation.Message))
            operation.Failed("امکان ثبت رکورد تکراری وجود ندارد. لطفا مجددا تلاش بفرمایید.");

        var slug = command.Slug.Slugify();

        var productCategory = new ProductCategory(command.Name, command.Description, command.Picture,
            command.PictureAlt, command.PictureTitle, command.Keyword, command.MetaDescription, slug);
        
        _repository.Create(productCategory);
        _repository.Save();
        return operation.Succeeded();
    }

    public OperationResult Edit(EditProductCategory command)
    {
        throw new NotImplementedException();
    }

    public List<ProductCategoryViewModel> Search(ProductCategorySearchModel command)
    {
        throw new NotImplementedException();
    }

    public ProductCategory GetDetails(long id)
    {
        throw new NotImplementedException();
    }
}