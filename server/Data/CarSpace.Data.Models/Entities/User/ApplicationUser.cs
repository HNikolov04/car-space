using CarSpace.Data.Models.CarForum;
using CarSpace.Data.Models.Entities.CarForum;
using CarSpace.Data.Models.Entities.CarMeet;
using CarSpace.Data.Models.Entities.CarService;
using CarSpace.Data.Models.Entities.CarShop;
using Microsoft.AspNetCore.Identity;

namespace CarSpace.Data.Models.Entities.User;

public class ApplicationUser : IdentityUser<Guid>
{
    public string ImageUrl { get; set; }
    public bool IsDeleted { get; set; }

    private ApplicationUser() { }

    public ApplicationUser(Guid id, string email, string username, string imageUrl)
    {
        Id = id;
        Email = email;
        UserName = username;
        ImageUrl = imageUrl;
    }

    public virtual ICollection<CarForumArticle> CreatedCarForumArticles { get; set; } = new HashSet<CarForumArticle>();
    public virtual ICollection<UserSavedCarForumArticle> SavedCarForumArticles { get; set; } = new HashSet<UserSavedCarForumArticle>();
    public virtual ICollection<CarForumComment> CreatedCarForumComments { get; set; } = new HashSet<CarForumComment>();

    public virtual ICollection<CarServiceListing> CreatedCarServiceListings { get; set; } = new HashSet<CarServiceListing>();
    public virtual ICollection<UserSavedCarServiceListing> SavedServiceListings { get; set; } = new HashSet<UserSavedCarServiceListing>();

    public virtual ICollection<CarMeetListing> CreatedCarMeetListings { get; set; } = new HashSet<CarMeetListing>();
    public virtual ICollection<UserJoinedCarMeetListing> JoinedCarMeetListings { get; set; } = new HashSet<UserJoinedCarMeetListing>();
    public virtual ICollection<UserSavedCarMeetListing> SavedCarMeetListings { get; set; } = new HashSet<UserSavedCarMeetListing>();

    public virtual ICollection<CarShopListing> CreatedCarShopListings { get; set; } = new HashSet<CarShopListing>();
    public virtual ICollection<UserSavedCarShopListing> SavedCarShopListings { get; set; } = new HashSet<UserSavedCarShopListing>();
}
