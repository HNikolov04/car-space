using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CarSpace.Data.Models.Entities.CarMeet;

namespace CarSpace.Data.Configurations.CarMeet;

public class UserSavedCarMeetListingConfiguration : IEntityTypeConfiguration<UserSavedCarMeetListing>
{
    public void Configure(EntityTypeBuilder<UserSavedCarMeetListing> builder)
    {
        builder.HasKey(s => new { s.UserId, s.CarMeetId });

        builder.HasOne(s => s.User)
               .WithMany(u => u.SavedCarMeetListings)
               .HasForeignKey(s => s.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.CarMeetListing)
               .WithMany(m => m.SavedByUsers)
               .HasForeignKey(s => s.CarMeetId)
               .OnDelete(DeleteBehavior.Cascade); 
    }
}