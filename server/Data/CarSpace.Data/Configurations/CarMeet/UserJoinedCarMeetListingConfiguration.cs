using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CarSpace.Data.Models.Entities.CarMeet;

namespace CarSpace.Data.Configurations.CarMeet;

public class UserJoinedCarMeetListingConfiguration : IEntityTypeConfiguration<UserJoinedCarMeetListing>
{
    public void Configure(EntityTypeBuilder<UserJoinedCarMeetListing> builder)
    {
        builder.HasKey(p => new { p.UserId, p.CarMeetId });

        builder.HasOne(p => p.User)
               .WithMany(u => u.JoinedCarMeetListings)
               .HasForeignKey(p => p.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.CarMeetListing)
               .WithMany(m => m.Participants)
               .HasForeignKey(p => p.CarMeetId)
               .OnDelete(DeleteBehavior.Cascade); 
    }
}
