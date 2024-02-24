using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.Business.Models;

namespace Payment.Data.Mapping
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasMany(c => c.OrderItems)
                .WithOne(c => c.Order)
                .HasForeignKey(c => c.OrderId);

            builder.ToTable("Orders");
        }
    }
}
