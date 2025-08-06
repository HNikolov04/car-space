using CarSpace.Data.Models.Entities.About;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Data.Configurations.About;

public class AboutUsConfiguration : IEntityTypeConfiguration<AboutUs>
{
    public void Configure(EntityTypeBuilder<AboutUs> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Message)
            .IsRequired()
            .HasMaxLength(1000);
    }
}
