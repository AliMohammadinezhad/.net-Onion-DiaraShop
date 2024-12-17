﻿using Framework.Application;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Application.Contract.Inventory;

public class IncreaseInventory
{
    public long InventoryId { get; set; }
    [Range(0, 100000000, ErrorMessage = ValidationMessages.IsPositive)]
    public long Count { get; set; }
    public string Description { get; set; }
}