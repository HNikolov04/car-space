using CarSpace.Data.Models.Entities.CarShop;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Configurations.CarShopConfigurations;

public class CarsAndSuvsListingConfiguration : IEntityTypeConfiguration<CarsAndSuvsListing>
{
    public void Configure(EntityTypeBuilder<CarsAndSuvsListing> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.Brand)
            .IsRequired()
            .HasMaxLength(50);

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

        builder.Property(x => x.ImageUrl)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Price)
            .HasPrecision(18, 2);

        builder.HasOne(x => x.User)
            .WithMany(u => u.CarsAndSuvsListings)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}
