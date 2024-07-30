using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockSmart.Domain.Entities;

namespace StockSmart.Infrastructure.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        builder.ToTable("Product");

        builder.HasKey(x => x.ProductId);

        builder
            .Property(p => p.ProductId)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(p => p.ProductCode)
            .HasMaxLength(30)
            .IsRequired();

        builder
            .HasIndex(i => new { i.ProductCode })
            .IsUnique();

        builder
            .Property(p => p.Name)
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(p => p.Stock)
            .IsRequired();

        builder
            .Property(p => p.Description)
            .HasMaxLength(5000)
            .IsRequired();

        builder
            .Property(f => f.Price)
            .HasColumnType("decimal(16,2)")
            .IsRequired();

        builder
            .Property(f => f.Discount)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder
           .Property(f => f.Weight)
           .HasColumnType("decimal(16,4)");

        builder
           .Property(f => f.Size)
           .HasColumnType("decimal(16,4)");

        builder.HasOne(p => p.Status)
            .WithMany()
            .HasForeignKey(nameof(Product.Status.StatusId))
            .IsRequired();
    }
}
