using CarSpace.Data.Models.Entities.User;

namespace CarSpace.Data.Models.Entities.CarService;

public class CarServiceListing
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public int CategoryId { get; set; }
    public virtual CarServiceCategory Category { get; set; } = null!;

    public Guid UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;

    public virtual ICollection<UserSavedCarServiceListing> SavedByUsers { get; set; } = new HashSet<UserSavedCarServiceListing>();
}
