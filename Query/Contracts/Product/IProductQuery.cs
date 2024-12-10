using ShopManagement.Contracts.Order;

namespace Query.Contracts.Product;

public interface IProductQuery
{
    ProductQueryModel GetProductDetails(string slug);
    List<ProductQueryModel> GetLatestArrivals();
    List<ProductQueryModel> Search(string value);
    List<CartItem> CheckInventoryStatus(List<CartItem> cartItems);
}