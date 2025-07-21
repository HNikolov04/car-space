using CarSpace.Data.Models.Entities.User;

namespace CarSpace.Data.Models.Entities.Meet;

public class CarMeet
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime MeetDate { get; set; }
    public string City { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public Guid CreatorId { get; set; }
    public ApplicationUser Creator { get; set; } = null!;

    public ICollection<ApplicationUser> Participants { get; set; } = new HashSet<ApplicationUser>();
}
