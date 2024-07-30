using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockSmart.Domain.Entities;

namespace StockSmart.Infrastructure.Configuration;

public class StatusConfiguration : IEntityTypeConfiguration<Status>
{
    public void Configure(EntityTypeBuilder<Status> builder)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        builder.ToTable("Status");

        builder.HasKey(x => x.StatusId);


        builder
            .Property(p => p.StatusId)
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(p => p.Name)
            .HasMaxLength(500)
            .IsRequired();

        builder
            .HasIndex(i => new { i.Name })
            .IsUnique();
    }
}
