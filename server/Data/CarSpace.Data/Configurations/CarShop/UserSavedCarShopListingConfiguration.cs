using CarSpace.Data.Models.Entities.CarShop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarSpace.Data.Configurations.CarShop;

public class UserSavedCarShopListingConfiguration : IEntityTypeConfiguration<UserSavedCarShopListing>
{
    public void Configure(EntityTypeBuilder<UserSavedCarShopListing> builder)
    {
        builder.HasKey(x => new { x.UserId, x.CarShopListingId });

        builder.HasOne(x => x.User)
            .WithMany(u => u.SavedCarShopListings)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CarShopListing)
            .WithMany(l => l.SavedByUsers)
            .HasForeignKey(x => x.CarShopListingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

