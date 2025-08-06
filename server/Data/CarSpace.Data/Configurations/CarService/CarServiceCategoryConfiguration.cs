using CarSpace.Data.Models.Entities.CarService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarSpace.Data.Configurations.CarService;

public class CarServiceCategoryConfiguration : IEntityTypeConfiguration<CarServiceCategory>
{
    public void Configure(EntityTypeBuilder<CarServiceCategory> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasMany(c => c.CarServiceListings)
               .WithOne(l => l.Category)
               .HasForeignKey(l => l.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
