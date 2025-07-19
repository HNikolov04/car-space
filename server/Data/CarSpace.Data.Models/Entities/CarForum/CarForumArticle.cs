using CarSpace.Data.Models.Entities.CarForum;
using CarSpace.Data.Models.Entities.User;

namespace CarSpace.Data.Models.CarForum;

public class CarForumArticle
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }
    public ICollection<CarForumComment> Comments { get; set; } = new HashSet<CarForumComment>();
}
