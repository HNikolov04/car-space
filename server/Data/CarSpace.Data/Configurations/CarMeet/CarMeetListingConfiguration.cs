using CarSpace.Data.Models.Entities.CarMeet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarSpace.Data.Configurations.CarMeet;

public class CarMeetListingConfiguration : IEntityTypeConfiguration<CarMeetListing>
{
    public void Configure(EntityTypeBuilder<CarMeetListing> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(m => m.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.ImageUrl)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(m => m.MeetDate)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .IsRequired();

        builder.HasOne(m => m.User)
            .WithMany(u => u.CreatedCarMeetListings)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
