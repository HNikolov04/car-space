using CarSpace.Data.Models.Entities.User;

namespace CarSpace.Data.Models.Entities.CarService;

public class UserSavedCarServiceListing
{
    public Guid UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;

    public Guid CarServiceListingId { get; set; }
    public virtual CarServiceListing CarServiceListing { get; set; } = null!;
}
