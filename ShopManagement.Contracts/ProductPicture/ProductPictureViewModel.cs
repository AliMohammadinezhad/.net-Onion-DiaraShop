﻿using Framework.Application;
using Microsoft.AspNetCore.Http;

namespace ShopManagement.Contracts.ProductPicture;

public class ProductPictureViewModel
{
    public long Id { get; set; }
    public string Product { get; set; }
    public string Picture { get; set; }
    public string CreationDate { get; set; }
    public long ProductId { get; set; }
    public bool IsRemoved { get; set; }
}