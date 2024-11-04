using Microsoft.AspNetCore.Mvc;
using Query.Contracts.Product;

namespace ServiceHost.ViewComponents;

public class LatestArrivalViewComponent : ViewComponent
{
    private readonly IProductQuery _productQuery;

    public LatestArrivalViewComponent(IProductQuery productQuery)
    {
        _productQuery = productQuery;
    }

    public IViewComponentResult Invoke()
    {
        var products = _productQuery.GetLatestArrivals();
        return View(products);
    }
}