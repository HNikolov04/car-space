namespace CarSpace.Data.Models.Entities.Contact;

public class ContactUs
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

