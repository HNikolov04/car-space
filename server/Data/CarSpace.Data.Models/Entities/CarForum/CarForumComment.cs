using CarSpace.Data.Models.CarForum;
using CarSpace.Data.Models.Entities.User;

namespace CarSpace.Data.Models.Entities.CarForum;

public class CarForumComment
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }
    public Guid CarForumArticleId { get; set; }
    public CarForumArticle CarForumArticle { get; set; }
}
