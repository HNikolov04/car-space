namespace CarSpace.Data.Models.Entities.CarShop;

public class CarShopBrand
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<CarShopListing> CarShopListings { get; set; } = new HashSet<CarShopListing>();
}