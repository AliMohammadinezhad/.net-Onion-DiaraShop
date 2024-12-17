
using Microsoft.AspNetCore.Mvc;
using Query.Contracts.Product;

namespace ShopManagement.Presentation.Api;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductQuery _productQuery;

    public ProductController(IProductQuery productQuery)
    {
        _productQuery = productQuery;
    }


    [HttpGet]
    public List<ProductQueryModel> GetLatestArrivals()
    {
        return _productQuery.GetLatestArrivals();
    }
}