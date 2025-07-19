using CarSpace.Data.Models.CarForum;
using CarSpace.Data.Models.Entities.CarForum;
using Microsoft.AspNetCore.Identity;

namespace CarSpace.Data.Models.Entities.User;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    private ApplicationUser() { }

    public ApplicationUser(string email, string username)
    {
        Id = Guid.NewGuid();
        Email = email;
        UserName = username;
    }

    public ICollection<CarForumArticle> CarForumArticles { get; set; } = new HashSet<CarForumArticle>();
    public ICollection<CarForumComment> CarForumComments { get; set; } = new HashSet<CarForumComment>();
}
