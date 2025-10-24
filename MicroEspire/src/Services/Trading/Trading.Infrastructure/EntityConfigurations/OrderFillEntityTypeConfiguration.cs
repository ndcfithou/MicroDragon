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
    public class OrderFillEntityTypeConfiguration : IEntityTypeConfiguration<OrderFill>
    {
        public void Configure(EntityTypeBuilder<OrderFill> builder)
        {
            builder.ToTable("OrderFills");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.OrderId).IsRequired();
            builder.Property(f => f.Quantity).HasPrecision(18, 8).IsRequired();
            builder.Property(f => f.Price).HasPrecision(18, 8).IsRequired();
            builder.Property(f => f.Fee).HasPrecision(18, 8).IsRequired();
            builder.Property(f => f.FilledAt).IsRequired();

            builder.HasIndex(f => f.OrderId);
            builder.HasIndex(f => f.FilledAt);
        }
    }
}
