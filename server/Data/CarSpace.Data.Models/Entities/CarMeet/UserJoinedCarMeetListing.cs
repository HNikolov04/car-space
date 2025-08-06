using CarSpace.Data.Models.Entities.User;

namespace CarSpace.Data.Models.Entities.CarMeet;

public class UserJoinedCarMeetListing
{
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public Guid CarMeetId { get; set; }
    public CarMeetListing CarMeetListing { get; set; } = null!;
}
