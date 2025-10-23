using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Domain.AggregatesModel.OrderAggregate;

namespace Trading.Infrastructure.EntityConfigurations
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.UserId).IsRequired();
            builder.Property(o => o.Symbol).IsRequired().HasMaxLength(20);
            builder.Property(o => o.Type).IsRequired();
            builder.Property(o => o.Side).IsRequired();
            builder.Property(o => o.Quantity).HasPrecision(18, 8).IsRequired();
            builder.Property(o => o.Price).HasPrecision(18, 8).IsRequired();
            builder.Property(o => o.StopPrice).HasPrecision(18, 8);
            builder.Property(o => o.Status).IsRequired();
            builder.Property(o => o.FilledQuantity).HasPrecision(18, 8).IsRequired();
            builder.Property(o => o.AveragePrice).HasPrecision(18, 8).IsRequired();
            builder.Property(o => o.CreatedAt).IsRequired();

            builder.HasMany(o => o.Fills)
                .WithOne()
                .HasForeignKey("OrderId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(o => o.UserId);
            builder.HasIndex(o => o.Symbol);
            builder.HasIndex(o => o.Status);
            builder.HasIndex(o => o.CreatedAt);

            builder.Ignore(o => o.DomainEvents);
        }
    }
}
