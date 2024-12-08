namespace DiscountManagement.infrastructure.Configuration.Permissions;

public static class DiscountPermissions
{
    // Customer Discount
    public const int ListCustomerDiscount = 60;
    public const int SearchCustomerDiscount = 61;
    public const int CreateCustomerDiscount = 62;
    public const int EditCustomerDiscount = 63;


    // Colleague Discount
    public const int ListColleagueDiscount = 70;
    public const int SearchColleagueDiscount = 71;
    public const int CreateColleagueDiscount = 72;
    public const int EditColleagueDiscount = 73;
    public const int RemoveColleagueDiscount = 74;
    public const int RestoreColleagueDiscount = 75;
}