namespace CarSpace.Data.Models.Entities.About;

public class AboutUs
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
