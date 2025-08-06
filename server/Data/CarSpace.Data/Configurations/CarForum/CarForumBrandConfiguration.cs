using CarSpace.Data.Models.Entities.CarForum;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Configurations.CarForum;

public class CarForumBrandConfiguration : IEntityTypeConfiguration<CarForumBrand>
{
    public void Configure(EntityTypeBuilder<CarForumBrand> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(b => b.CarForumArticles)
            .WithOne(a => a.CarBrand)
            .HasForeignKey(a => a.CarBrandId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}
