using ShopManagement.Contracts.Order;

namespace Query.Contracts;

public interface ICartCalculatorService
{
    Cart ComputeCart(List<CartItem> cartItems);
}