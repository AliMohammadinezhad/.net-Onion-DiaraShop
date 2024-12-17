using Framework.Application;
using Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Contracts.Product;
using ShopManagement.Contracts.ProductPicture;
using ShopManagement.infrastructure.Configuration.Permissions;

namespace ServiceHost.Areas.Administration.Pages.Shop.ProductPictures;
public class IndexModel : PageModel
{
    [TempData] public string Message { get; set; }
    public ProductPictureSearchModel SearchModel;
    public SelectList Products;
    public List<ProductPictureViewModel> ProductPictures;

    private readonly IProductPictureApplication _productPictureApplication;
    private readonly IProductApplication _productApplication;

    public IndexModel(IProductPictureApplication productPictureApplication, IProductApplication productApplication)
    {
        _productPictureApplication = productPictureApplication;
        _productApplication = productApplication;
    }

    [NeedsPermission(ShopPermissions.ListProductPictures)]
    public void OnGet(ProductPictureSearchModel searchModel)
    {
        Products = new SelectList(_productApplication.GetProducts(), "Id", "Name");
        ProductPictures = _productPictureApplication.Search(searchModel);
    }

    public IActionResult OnGetCreate()
    {
        var command = new CreateProductPicture
        {
            Products = _productApplication.GetProducts()
        };
        return Partial("./Create", command);
    }

    public IActionResult OnPostCreate(CreateProductPicture command)
    {
        var result = _productPictureApplication.Create(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetEdit(long id)
    {
        var productPicture = _productPictureApplication.GetDetails(id);
        productPicture.Products = _productApplication.GetProducts();
        return Partial("./Edit", productPicture);
    }

    public JsonResult OnPostEdit(EditProductPicture command)
    {
        var result = _productPictureApplication.Edit(command);
        return new JsonResult(result);
    }

    public RedirectToPageResult OnGetRemoved(long id)
    {
        OperationResult result = _productPictureApplication.Remove(id);
        if (result.IsSucceeded)
            return RedirectToPage("./Index");
        Message = result.Message;
        return RedirectToPage("./Index");
    }

    public RedirectToPageResult OnGetRestore(long id)
    {
        OperationResult result = _productPictureApplication.Restore(id);
        if (result.IsSucceeded)
            return RedirectToPage("./Index");
        Message = result.Message;
        return RedirectToPage("./Index");
    }
}

