using System.ComponentModel.DataAnnotations;
using Framework.Application;

namespace ShopManagement.Contracts.ProductCategory;

public class CreateProductCategory
{
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Name { get;  set; }
    public string? Description { get;  set; }
    public string? Picture { get;  set; }
    public string? PictureAlt { get;  set; }
    public string? PictureTitle { get;  set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Keyword { get;  set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string MetaDescription { get;  set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Slug { get;  set; }
}