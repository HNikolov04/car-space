using CarSpace.Data.Models.CarForum;
using CarSpace.Data.Models.Entities.CarForum;
using CarSpace.Data.Models.Entities.CarServices;
using CarSpace.Data.Models.Entities.CarShop;
using CarSpace.Data.Models.Entities.Meet;
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

    public ICollection<CarServiceListing> CarServiceListings { get; set; } = new HashSet<CarServiceListing>();

    public ICollection<CarMeet> CreatedMeets { get; set; } = new HashSet<CarMeet>();
    public ICollection<CarMeet> JoinedMeets { get; set; } = new HashSet<CarMeet>();

    public ICollection<CarsAndSuvsListing> CarsAndSuvsListings { get; set; } = new HashSet<CarsAndSuvsListing>();
}
