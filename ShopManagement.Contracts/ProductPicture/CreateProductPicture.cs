using Framework.Application;
using Microsoft.AspNetCore.Http;
using ShopManagement.Contracts.Product;
using System.ComponentModel.DataAnnotations;

namespace ShopManagement.Contracts.ProductPicture;

public class CreateProductPicture
{
    [Range(1, 1000000, ErrorMessage = ValidationMessages.IsRequired)]
    public long ProductId { get; set; }
    [FileExtensionLimitation([".jpeg", ".jpg", ".png"], ErrorMessage = ValidationMessages.InvalidFileFormat)]
    [MaxFileSize(3 * 1024 * 1024, ErrorMessage = ValidationMessages.IsMaxFileSize)]
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public IFormFile Picture { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string PictureAlt { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string PictureTitle { get; set; }
    public List<ProductViewModel> Products { get; set; }
}