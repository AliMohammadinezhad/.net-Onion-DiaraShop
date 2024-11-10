using System.ComponentModel.DataAnnotations;
using Framework.Application;
using Microsoft.AspNetCore.Http;

namespace ShopManagement.Contracts.Slide;

public class CreateSlide
{
    [FileExtensionLimitation([".jpeg", ".jpg", ".png"], ErrorMessage = ValidationMessages.InvalidFileFormat)]
    [MaxFileSize(3 * 1024 * 1024, ErrorMessage = ValidationMessages.IsMaxFileSize)]
    public IFormFile? Picture { get;  set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string PictureAlt { get;  set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string PictureTitle { get;  set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Heading { get;  set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Title { get;  set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Text { get;  set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string BtnText { get;  set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Link { get; set; }
}