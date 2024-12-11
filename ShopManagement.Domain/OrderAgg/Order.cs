using Framework.Domain;

namespace ShopManagement.Domain.OrderAgg;

public class Order : EntityBase
{
    public long AccountId { get; private set; }
    public double TotalPrice { get; private set; }
    public double DiscountAmount { get; private set; }
    public double PayAmount { get; private set; }
    public bool IsPaid { get; private set; }
    public bool IsCancelled { get; private set; }
    public string IssueTrackingNumber { get; private set; }
    public long RefId { get; private set; }
    public List<OrderItem> Items { get; private set; }

    public Order(long accountId, double totalPrice, double discountAmount, double payAmount)
    {
        AccountId = accountId;
        TotalPrice = totalPrice;
        DiscountAmount = discountAmount;
        PayAmount = payAmount;
        IsPaid = false;
        IsCancelled = false;
        RefId = 0;
        Items = [];
    }

    public void PaymentSucceeded(long refId)
    {
        IsPaid = true;
        
        if(refId !=  0)
            RefId = refId;

    }

    public void Cancel()
    {
        IsCancelled = true;
    }

    public void SetIssueTrackingNumber(string number)
    {
        IssueTrackingNumber = number;
    }

    public void AddItem(OrderItem item)
    {
        Items.Add(item);
    }


}