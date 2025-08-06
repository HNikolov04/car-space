using CarSpace.Data.Models.Entities.CarForum;
using CarSpace.Data.Models.Entities.User;

namespace CarSpace.Data.Models.CarForum;

public class CarForumArticle
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Guid UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;

    public int CarBrandId { get; set; }
    public virtual CarForumBrand CarBrand { get; set; } = null!;

    public virtual ICollection<CarForumComment> CarForumComments { get; set; } = new HashSet<CarForumComment>();
    public virtual ICollection<UserSavedCarForumArticle> SavedByUsers { get; set; } = new HashSet<UserSavedCarForumArticle>();
}
