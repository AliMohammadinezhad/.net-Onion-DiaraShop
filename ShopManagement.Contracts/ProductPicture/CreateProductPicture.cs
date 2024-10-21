using System.ComponentModel.DataAnnotations;
using Framework.Application;
using ShopManagement.Contracts.Product;

namespace ShopManagement.Contracts.ProductPicture;

public class CreateProductPicture
{
    [Range(1,1000000, ErrorMessage = ValidationMessages.IsRequired)]
    public long ProductId { get;  set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Picture { get;  set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string PictureAlt { get;  set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string PictureTitle { get;  set; }
    public List<ProductViewModel> Products { get; set; }
}