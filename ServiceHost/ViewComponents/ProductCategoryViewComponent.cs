using Microsoft.AspNetCore.Mvc;
using Query.Contracts.ProductCategory;

namespace ServiceHost.ViewComponents;

public class ProductCategoryViewComponent : ViewComponent
{
    private readonly IProductCategoryQuery productCategoryQuery;

    public ProductCategoryViewComponent(IProductCategoryQuery productCategoryQuery)
    {
        this.productCategoryQuery = productCategoryQuery;
    }

    public IViewComponentResult Invoke()
    {
        var productCategoryQueryModels = productCategoryQuery.GetProductCategories();
        return View(productCategoryQueryModels);
    }
}