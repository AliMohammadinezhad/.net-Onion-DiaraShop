using DiscountManagement.Application.Contract.CustomerDiscount;
using DiscountManagement.infrastructure.Configuration.Permissions;
using Framework.Application;
using Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Contracts.Product;
using ShopManagement.Contracts.ProductCategory;
using ShopManagement.Domain.ProductAgg;

namespace ServiceHost.Areas.Administration.Pages.Discounts.CustomerDiscount
{
    public class IndexModel : PageModel
    {
        [TempData] public string Message { get; set; }
        public CustomerDiscountSearchModel SearchModel;
        public SelectList Products;
        public List<CustomerDiscountViewModel> CustomerDiscount;
        private readonly IProductApplication _productApplication;
        private readonly ICustomerDiscountApplication _customerDiscountApplication;
        public IndexModel(IProductApplication productApplication, ICustomerDiscountApplication customerDiscountApplication)
        {
            _productApplication = productApplication;
            _customerDiscountApplication = customerDiscountApplication;
        }

        [NeedsPermission(DiscountPermissions.ListCustomerDiscount)]
        public void OnGet(CustomerDiscountSearchModel searchModel)
        {
            Products = new SelectList(_productApplication.GetProducts(), "Id", "Name");
            CustomerDiscount = _customerDiscountApplication.Search(searchModel);
        }

        [NeedsPermission(DiscountPermissions.CreateCustomerDiscount)]
        public IActionResult OnGetCreate()        
        {
            var command = new DefineCustomerDiscount
            {
                Products = _productApplication.GetProducts()

            };
            return Partial("./Create", command);
        }

        [NeedsPermission(DiscountPermissions.CreateCustomerDiscount)]
        public IActionResult OnPostCreate(DefineCustomerDiscount command)
        {
            var result = _customerDiscountApplication.Define(command);
            return new JsonResult(result);
        }

        [NeedsPermission(DiscountPermissions.EditCustomerDiscount)]
        public IActionResult OnGetEdit(long id)
        {
            var customerDiscount = _customerDiscountApplication.GetDetails(id);
            customerDiscount.Products = _productApplication.GetProducts();
            return Partial("./Edit", customerDiscount);
        }

        [NeedsPermission(DiscountPermissions.EditCustomerDiscount)]
        public JsonResult OnPostEdit(EditCustomerDiscount command)
        {
            var result = _customerDiscountApplication.Edit(command);
            return new JsonResult(result);
        }

    }
}
