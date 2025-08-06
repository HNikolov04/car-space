using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CarSpace.Data.Models.Entities.CarService;

namespace CarSpace.Data.Configurations.CarService;

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
               .HasMaxLength(500);

        builder.Property(x => x.PhoneNumber)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(x => x.City)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Address)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.Price)
               .HasPrecision(18, 2);

        builder.Property(x => x.ImageUrl)
               .IsRequired()
               .HasMaxLength(300);

        builder.Property(x => x.CreatedAt)
               .IsRequired();

        builder.Property(x => x.UpdatedAt)
               .IsRequired();

        builder.HasOne(x => x.Category)
               .WithMany(c => c.CarServiceListings)
               .HasForeignKey(x => x.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
               .WithMany(u => u.CreatedCarServiceListings)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
