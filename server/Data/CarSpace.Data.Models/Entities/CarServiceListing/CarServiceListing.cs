using CarSpace.Data.Models.Entities.User;

namespace CarSpace.Data.Models.Entities.CarServiceListing;

public class CarServiceListing
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Category { get; set; } = null!; 
    public string PhoneNumber { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Address { get; set; } = null!;
    public decimal? Price { get; set; }  
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
}
