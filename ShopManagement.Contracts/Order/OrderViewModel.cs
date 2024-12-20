﻿namespace ShopManagement.Contracts.Order;

public class OrderViewModel
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public string AccountFullName { get; set; }
    public int PaymentMethodId { get; set; }
    public string PaymentMethod { get; set; }
    public double TotalPrice { get; set; }
    public double DiscountAmount { get; set; }
    public double PayAmount { get; set; }
    public bool IsPaid { get; set; }
    public bool IsCancelled { get; set; }
    public string IssueTrackingNumber { get; set; }
    public long RefId { get; set; }
    public string CreationDate { get; set; }
    public DateTime CreationDateCalculations { get; set; }
}