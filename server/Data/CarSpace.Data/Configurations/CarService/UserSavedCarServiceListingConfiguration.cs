using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CarSpace.Data.Models.Entities.CarService;

namespace CarSpace.Data.Configurations.CarService;

public class UserSavedCarServiceListingConfiguration : IEntityTypeConfiguration<UserSavedCarServiceListing>
{
    public void Configure(EntityTypeBuilder<UserSavedCarServiceListing> builder)
    {
        builder.HasKey(x => new { x.UserId, x.CarServiceListingId });

        builder.HasOne(x => x.User)
               .WithMany(u => u.SavedServiceListings)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CarServiceListing)
               .WithMany(l => l.SavedByUsers)
               .HasForeignKey(x => x.CarServiceListingId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
