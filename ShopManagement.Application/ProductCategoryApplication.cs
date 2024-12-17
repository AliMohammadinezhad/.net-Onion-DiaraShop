using Framework.Application;
using ShopManagement.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Application;

public class ProductCategoryApplication : IProductCategoryApplication
{
    private readonly IFileUploader _fileUploader;
    private readonly IProductCategoryRepository _repository;

    public ProductCategoryApplication(IProductCategoryRepository repository, IFileUploader fileUploader)
    {
        _repository = repository;
        _fileUploader = fileUploader;
    }

    public OperationResult Create(CreateProductCategory command)
    {
        var operation = new OperationResult();
        if (_repository.Exists(x => x.Name == command.Name))
            operation.Failed(ApplicationMessages.DuplicatedRecord);

        var slug = command.Slug.Slugify();
        var picturePath = command.Slug;
        var pictureName = _fileUploader.Upload(command.Picture, picturePath);

        var productCategory = new ProductCategory(command.Name, command.Description, pictureName,
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
        var picturePath = command.Slug;
        var fileName = _fileUploader.Upload(command.Picture, picturePath);

        productCategory.Edit(command.Name, command.Description,
            fileName, command.PictureAlt, command.PictureTitle,
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