﻿using Framework.Application;
using Microsoft.AspNetCore.Http;
using ShopManagement.Contracts.ProductCategory;
using System.ComponentModel.DataAnnotations;

namespace ShopManagement.Contracts.Product;

public class CreateProduct
{
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Name { get; set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Code { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    [FileExtensionLimitation([".jpeg", ".jpg", ".png"], ErrorMessage = ValidationMessages.InvalidFileFormat)]
    [MaxFileSize(3 * 1024 * 1024, ErrorMessage = ValidationMessages.IsMaxFileSize)]
    public IFormFile? Picture { get; set; }
    public string? PictureAlt { get; set; }
    public string? PictureTitle { get; set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Slug { get; set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Keyword { get; set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string MetaDescription { get; set; }
    [Range(1, 100000, ErrorMessage = ValidationMessages.IsRequired)]
    public long CategoryId { get; set; }
    public List<ProductCategoryViewModel> Categories { get; set; }
}