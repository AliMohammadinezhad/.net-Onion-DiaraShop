using System.ComponentModel.DataAnnotations;
using Framework.Application;
using Microsoft.AspNetCore.Http;

namespace ShopManagement.Contracts.ProductCategory;

public class CreateProductCategory
{
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Name { get;  set; }
    public string? Description { get;  set; }

    [FileExtensionLimitation([".jpeg", ".jpg", ".png"], ErrorMessage = ValidationMessages.InvalidFileFormat)]
    [MaxFileSize(3 * 1024 * 1024 , ErrorMessage = ValidationMessages.IsMaxFileSize)]
    public IFormFile? Picture { get;  set; }
    public string? PictureAlt { get;  set; }
    public string? PictureTitle { get;  set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Keyword { get;  set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string MetaDescription { get;  set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Slug { get;  set; }
}