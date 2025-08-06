using CarSpace.Data.Models.Entities.User;

namespace CarSpace.Data.Models.Entities.CarShop;

public class CarShopListing
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int Year { get; set; }
    public int Mileage { get; set; }
    public int Horsepower { get; set; }
    public string Transmission { get; set; } = null!;
    public string FuelType { get; set; } = null!;
    public string Color { get; set; } = null!;
    public string EuroStandard { get; set; } = null!;
    public int Doors { get; set; }
    public decimal Price { get; set; }
    public string City { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public int CarBrandId { get; set; }
    public virtual CarShopBrand Brand { get; set; } = null!;

    public Guid UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;

    public virtual ICollection<UserSavedCarShopListing> SavedByUsers { get; set; } = new HashSet<UserSavedCarShopListing>();
}