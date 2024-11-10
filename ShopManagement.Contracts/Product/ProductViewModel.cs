using Framework.Application;
using Microsoft.AspNetCore.Http;

namespace ShopManagement.Contracts.Product;

public class ProductViewModel
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string Code { get; set; }
    [FileExtensionLimitation([".jpeg", ".jpg", ".png"], ErrorMessage = ValidationMessages.InvalidFileFormat)]
    [MaxFileSize(3 * 1024 * 1024, ErrorMessage = ValidationMessages.IsMaxFileSize)]
    public IFormFile? Picture { get; set; }
    public string Category { get; set; }
    public string CreationDate { get; set; }
    public long CategoryId { get; set; }
}