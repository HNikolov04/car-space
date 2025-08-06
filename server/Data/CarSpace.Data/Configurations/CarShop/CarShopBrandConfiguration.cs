using CarSpace.Data.Models.Entities.CarShop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarSpace.Data.Configurations.CarShop;

public class CarShopBrandConfiguration : IEntityTypeConfiguration<CarShopBrand>
{
    public void Configure(EntityTypeBuilder<CarShopBrand> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(b => b.CarShopListings)
            .WithOne(l => l.Brand)
            .HasForeignKey(l => l.CarBrandId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
