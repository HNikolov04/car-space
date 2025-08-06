using CarSpace.Data.Models.Entities.User;

namespace CarSpace.Data.Models.Entities.CarShop;

public class UserSavedCarShopListing
{
    public Guid UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;

    public Guid CarShopListingId { get; set; }
    public virtual CarShopListing CarShopListing { get; set; } = null!;
}
