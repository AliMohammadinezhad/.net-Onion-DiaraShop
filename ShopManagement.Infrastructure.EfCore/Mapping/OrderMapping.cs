﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagement.Domain.OrderAgg;

namespace ShopManagement.Infrastructure.EfCore.Mapping;

public class OrderMapping : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.IssueTrackingNumber).HasMaxLength(8).IsRequired(false);
        builder.OwnsMany(x => x.Items, navigationBuilder =>
        {
            navigationBuilder.ToTable("OrderItems");
            navigationBuilder.HasKey(x => x.Id);
            navigationBuilder.WithOwner(x => x.Order).HasForeignKey(x => x.OrderId);

        });
    }
}