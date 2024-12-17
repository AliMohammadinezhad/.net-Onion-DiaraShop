using Framework.Application;
using ShopManagement.Contracts.Product;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Application;

public class ProductApplication : IProductApplication
{
    private readonly IProductRepository _repository;
    private readonly IFileUploader _fileUploader;
    private readonly IProductCategoryRepository _categoryRepository;
    public ProductApplication(IProductRepository repository, IFileUploader fileUploader, IProductCategoryRepository categoryRepository)
    {
        _repository = repository;
        _fileUploader = fileUploader;
        _categoryRepository = categoryRepository;
    }

    public OperationResult Create(CreateProduct command)
    {
        var operation = new OperationResult();
        if (_repository.Exists(x => x.Name == command.Name))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var slug = command.Slug.Slugify();
        var categorySlug = _categoryRepository.GetCategorySlugById(command.CategoryId);
        var path = $"{categorySlug}/{slug}";
        var picturePath = _fileUploader.Upload(command.Picture, path);
        var product = new Product(command.Name, command.Code, command.ShortDescription,
            command.Description, picturePath, command.PictureAlt, command.PictureTitle, slug, command.Keyword,
            command.MetaDescription, command.CategoryId);
        _repository.Create(product);
        _repository.SaveChanges();

        return operation.Succeeded();
    }

    public OperationResult Edit(EditProduct command)
    {
        var operation = new OperationResult();
        var product = _repository.GetProductWithCategoryById(command.Id);
        if (product == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);

        if (_repository.Exists(x => x.Name == command.Name && x.Id != command.Id))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var slug = command.Slug.Slugify();
        var path = $"{product.Category.Slug}/{slug}";
        var picturePath = _fileUploader.Upload(command.Picture, path);
        product.Edit(command.Name, command.Code, command.ShortDescription,
            command.Description, picturePath, command.PictureAlt, command.PictureTitle, slug,
            command.Keyword,
            command.MetaDescription, command.CategoryId);

        _repository.SaveChanges();
        return operation.Succeeded();
    }


    public EditProduct GetDetails(long id)
    {
        return _repository.GetDetails(id);
    }

    public List<ProductViewModel> GetProducts()
    {
        return _repository.GetProducts();
    }

    public List<ProductViewModel> Search(ProductSearchModel searchModel)
    {
        return _repository.Search(searchModel);
    }
}