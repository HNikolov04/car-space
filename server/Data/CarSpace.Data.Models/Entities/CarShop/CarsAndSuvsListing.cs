using CarSpace.Data.Models.Entities.User;

namespace CarSpace.Data.Models.Entities.CarShop;

public class CarsAndSuvsListing
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Brand { get; set; } = null!;
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
    public string ImageUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
}