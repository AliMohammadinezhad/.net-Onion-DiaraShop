using Microsoft.AspNetCore.Mvc;
using Query;
using Query.Contracts.ArticleCategory;
using Query.Contracts.ProductCategory;

namespace ServiceHost.ViewComponents;

public class MenuViewComponent : ViewComponent
{
    private readonly IProductCategoryQuery _productCategory;
    private readonly IArticleCategoryQuery _articleCategory;
    public MenuViewComponent(IProductCategoryQuery productCategory, IArticleCategoryQuery articleCategory)
    {
        _productCategory = productCategory;
        _articleCategory = articleCategory;
    }

    public IViewComponentResult Invoke()
    {
        var result = new MenuModel
        {
            ArticleCategories = _articleCategory.GetArticleCategories(),
            productCategories = _productCategory.GetProductCategories()
        };
        return View(result);
    }
}