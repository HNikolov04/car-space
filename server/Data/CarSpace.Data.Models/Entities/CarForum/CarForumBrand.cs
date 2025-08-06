using CarSpace.Data.Models.CarForum;

namespace CarSpace.Data.Models.Entities.CarForum;

public class CarForumBrand
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<CarForumArticle> CarForumArticles { get; set; } = new HashSet<CarForumArticle>();
}
