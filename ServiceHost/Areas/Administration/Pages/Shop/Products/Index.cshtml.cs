using Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Contracts.Product;
using ShopManagement.Contracts.ProductCategory;
using ShopManagement.infrastructure.Configuration.Permissions;

namespace ServiceHost.Areas.Administration.Pages.Shop.Products
{
    public class IndexModel : PageModel
    {
        [TempData] public string Message { get; set; }
        public ProductSearchModel SearchModel;
        public SelectList ProductCategories;
        public List<ProductViewModel> Products;
        private readonly IProductApplication _productApplication;
        private readonly IProductCategoryApplication _categoryApplication;
        public IndexModel(IProductApplication productApplication, IProductCategoryApplication categoryApplication)
        {
            _productApplication = productApplication;
            _categoryApplication = categoryApplication;
        }

        [NeedsPermission(ShopPermissions.ListProducts)]
        public void OnGet(ProductSearchModel searchModel)
        {
            ProductCategories = new SelectList(_categoryApplication.GetProductCategories(), "Id", "Name");
            Products = _productApplication.Search(searchModel);
        }

        [NeedsPermission(ShopPermissions.CreateProduct)]
        public IActionResult OnGetCreate()
        {
            var command = new CreateProduct
            {
                Categories = _categoryApplication.GetProductCategories()
            };
            return Partial("./Create", command);
        }

        [NeedsPermission(ShopPermissions.CreateProduct)]
        public IActionResult OnPostCreate(CreateProduct command)
        {
            var result = _productApplication.Create(command);
            return new JsonResult(result);
        }

        [NeedsPermission(ShopPermissions.EditProducts)]
        public IActionResult OnGetEdit(long id)
        {
            var product = _productApplication.GetDetails(id);
            product.Categories = _categoryApplication.GetProductCategories();
            return Partial("./Edit", product);
        }

        [NeedsPermission(ShopPermissions.EditProducts)]
        public JsonResult OnPostEdit(EditProduct command)
        {
            var result = _productApplication.Edit(command);
            return new JsonResult(result);
        }


    }
}
