using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CarSpace.Data.Models.Entities.CarServices;

namespace CarSpace.Data.Configurations.CarServiceListingConfigurations;

public class CarServiceListingConfiguration : IEntityTypeConfiguration<CarServiceListing>
{
    public void Configure(EntityTypeBuilder<CarServiceListing> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.Price)
            .HasPrecision(18, 2); 

        builder.Property(x => x.ImageUrl)
            .IsRequired()
            .HasMaxLength(300);
    }
}
