using ShopManagement.Contracts.Order;
using ShopManagement.Infrastructure.EfCore;

namespace Query.Contracts;

public interface ICartCalculatorService
{
    Cart ComputeCart(List<CartItem> cartItems);
}