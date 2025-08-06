using CarSpace.Data.Models.Entities.User;

namespace CarSpace.Data.Models.Entities.CarMeet;

public class CarMeetListing
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime MeetDate { get; set; }
    public string City { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Guid UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;

    public virtual ICollection<UserJoinedCarMeetListing> Participants { get; set; } = new HashSet<UserJoinedCarMeetListing>();
    public virtual ICollection<UserSavedCarMeetListing> SavedByUsers { get; set; } = new HashSet<UserSavedCarMeetListing>();
}
