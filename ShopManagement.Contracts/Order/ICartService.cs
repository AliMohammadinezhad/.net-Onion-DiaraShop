namespace ShopManagement.Contracts.Order;

public interface ICartService
{
    void Set(Cart cart);
    Cart Get();
}