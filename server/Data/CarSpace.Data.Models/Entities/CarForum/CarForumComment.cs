using CarSpace.Data.Models.CarForum;
using CarSpace.Data.Models.Entities.User;

namespace CarSpace.Data.Models.Entities.CarForum;

public class CarForumComment
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public Guid UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;

    public Guid CarForumArticleId { get; set; }
    public virtual CarForumArticle CarForumArticle { get; set; } = null!;
}
