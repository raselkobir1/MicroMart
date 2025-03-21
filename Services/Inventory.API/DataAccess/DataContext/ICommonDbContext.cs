﻿using Inventory.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.DataAccess.DataContext
{
    public interface ICommonDbContext
    {
        DbSet<InventoryInfo> InventoryInfo { get; set; } 
        DbSet<InventoryHistory> InventoryHistory { get; set; }   
    }
}
