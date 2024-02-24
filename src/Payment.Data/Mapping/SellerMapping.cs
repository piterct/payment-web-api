using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.Business.Models;

namespace Payment.Data.Mapping
{
    public  class SellerMapping : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Cpf)
                .IsRequired()
                .HasColumnType("varchar(11)");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");


            builder.Property(x => x.Email)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(x => x.Phone)
                .IsRequired()
                .HasColumnType("varchar(15)");

            builder.ToTable("Sellers");
        }
    }
}
