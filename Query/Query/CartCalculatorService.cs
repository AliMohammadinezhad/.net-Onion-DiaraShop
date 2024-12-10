using DiscountManagement.Infrastructure.EfCore;
using Framework.Application;
using Framework.Infrastructure;
using Query.Contracts;
using ShopManagement.Contracts.Order;

namespace Query.Query;

public class CartCalculatorService : ICartCalculatorService
{
    private readonly IAuthHelper _authHelper;
    private readonly DiscountContext _discountContext;

    public CartCalculatorService(DiscountContext discountContext, IAuthHelper authHelper)
    {
        _discountContext = discountContext;
        _authHelper = authHelper;
    }

    public Cart ComputeCart(List<CartItem> cartItems)
    {
        var cart = new Cart();
        var colleagueDiscounts = _discountContext.ColleagueDiscounts
            .Where(x => !x.IsRemoved)
            .Select(x => new {x.DiscountRate, x.ProductId})
            .ToList();

        var customerDiscounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.DiscountRate, x.ProductId })
            .ToList();

        var currentAccountRole = _authHelper.CurrentAccountRole();


        foreach (var cartItem in cartItems)
        {
            if (currentAccountRole == Roles.Colleague)
            {
                var colleagueDiscount = colleagueDiscounts.FirstOrDefault(x => x.ProductId == cartItem.Id);
                if (colleagueDiscount != null) 
                    cartItem.DiscountRate = colleagueDiscount.DiscountRate;
            }
            else
            {
                var customerDiscount = customerDiscounts.FirstOrDefault(x => x.ProductId == cartItem.Id);
                if (customerDiscount != null) 
                    cartItem.DiscountRate = customerDiscount.DiscountRate;
            }

            cartItem.DiscountAmount = (cartItem.TotalItemPrice * cartItem.DiscountRate) / 100;
            cartItem.ItemPayAmount = cartItem.TotalItemPrice - cartItem.DiscountAmount;
            cart.Add(cartItem);
        }

        return cart;

    }
}