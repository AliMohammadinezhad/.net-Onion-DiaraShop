using System.ComponentModel.DataAnnotations;
using Framework.Application;
using ShopManagement.Contracts.Product;

namespace DiscountManagement.Application.Contract.ColleagueDiscount;

public class DefineColleagueDiscount
{
    [Range(1,1000000, ErrorMessage = ValidationMessages.IsRequired)]
    public long ProductId { get; set; }
    [Range(1, 99, ErrorMessage = ValidationMessages.IsRequired)]
    public int DiscountRate { get; set; }
    public List<ProductViewModel> Products { get; set; }
}