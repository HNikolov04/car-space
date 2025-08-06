using CarSpace.Data.Models.Entities.CarShop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarSpace.Data.Configurations.CarShop;

public class CarShopListingConfiguration : IEntityTypeConfiguration<CarShopListing>
{
    public void Configure(EntityTypeBuilder<CarShopListing> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.Model)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Transmission)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.FuelType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Color)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.EuroStandard)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ImageUrl)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Price)
            .HasPrecision(18, 2);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(u => u.CreatedCarShopListings)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Brand)
            .WithMany(b => b.CarShopListings)
            .HasForeignKey(x => x.CarBrandId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
