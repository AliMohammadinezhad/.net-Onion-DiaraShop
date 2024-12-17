using Framework.Application;
using ShopManagement.Contracts.ProductPicture;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductPictureAgg;

namespace ShopManagement.Application;

public class ProductPictureApplication : IProductPictureApplication
{
    private readonly IProductPictureRepository _repository;
    private readonly IProductRepository _productRepository;
    private readonly IFileUploader _fileUploader;
    public ProductPictureApplication(IProductPictureRepository repository,
        IProductRepository productRepository, IFileUploader fileUploader)
    {
        _repository = repository;
        _productRepository = productRepository;
        _fileUploader = fileUploader;
    }

    public OperationResult Create(CreateProductPicture command)
    {
        var operation = new OperationResult();

        var product = _productRepository.GetProductWithCategoryById(command.ProductId);
        var path = $"{product.Category.Slug}/{product.Slug}";
        var picturePath = _fileUploader.Upload(command.Picture, path);
        var productPicture =
            new ProductPicture(command.ProductId, picturePath, command.PictureAlt, command.PictureTitle);
        _repository.Create(productPicture);
        _repository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Edit(EditProductPicture command)
    {
        var operation = new OperationResult();
        var productPicture = _repository.GetWithProductAndCategoryById(command.Id);

        if (productPicture == null)
            operation.Failed(ApplicationMessages.RecordNotFound);

        var path = $"{productPicture.Product.Category.Slug}/{productPicture.Product.Slug}";
        var picturePath = _fileUploader.Upload(command.Picture, path);

        productPicture.Edit(
            command.ProductId,
            picturePath,
            command.PictureAlt,
            command.PictureTitle);

        _repository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Remove(long id)
    {
        var operation = new OperationResult();
        var productPicture = _repository.Get(id);

        if (productPicture == null)
            operation.Failed(ApplicationMessages.RecordNotFound);

        productPicture.Remove();
        _repository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Restore(long id)
    {
        var operation = new OperationResult();
        var productPicture = _repository.Get(id);

        if (productPicture == null)
            operation.Failed(ApplicationMessages.RecordNotFound);

        productPicture.Restore();
        _repository.SaveChanges();
        return operation.Succeeded();
    }

    public EditProductPicture GetDetails(long id)
    {
        return _repository.GetDetails(id);
    }

    public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel)
    {
        return _repository.Search(searchModel);
    }
}