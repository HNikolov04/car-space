namespace CarSpace.Data.Models.Entities.CarService;

public class CarServiceCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<CarServiceListing> CarServiceListings { get; set; } = new List<CarServiceListing>();
}
