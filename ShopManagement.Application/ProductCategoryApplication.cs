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
        if (_repository.Exists(x => x.Name == command.Name))
            operation.Failed(ApplicationMessages.DuplicatedRecord);

        var slug = command.Slug.Slugify();

        var productCategory = new ProductCategory(command.Name, command.Description, command.Picture,
            command.PictureAlt, command.PictureTitle, command.Keyword, command.MetaDescription, slug);

        _repository.Create(productCategory);
        _repository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Edit(EditProductCategory command)
    {
        var operation = new OperationResult();
        var productCategory = _repository.Get(command.Id);
        
        if (productCategory == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);
        
        if (_repository.Exists(x => x.Name == command.Name && x.Id != command.Id))
            return operation.Failed(ApplicationMessages.RecordNotFound);

        var slug = command.Slug.Slugify();
        
        productCategory.Edit(command.Name, command.Description,
            command.Picture, command.PictureAlt, command.PictureTitle,
            command.Keyword, command.MetaDescription, slug);
        _repository.SaveChanges();
        return operation.Succeeded();
    }

    public List<ProductCategoryViewModel> GetProductCategories()
    {
        return _repository.GetProductCategories();
    }

    public List<ProductCategoryViewModel> Search(ProductCategorySearchModel command)
    {
        return _repository.Search(command);
    }

    public EditProductCategory GetDetails(long id)
    {
        return _repository.GetDetails(id);
    }
}