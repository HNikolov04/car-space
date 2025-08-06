using CarSpace.Data.Models.CarForum;
using CarSpace.Data.Models.Entities.About;
using CarSpace.Data.Models.Entities.CarForum;
using CarSpace.Data.Models.Entities.CarMeet;
using CarSpace.Data.Models.Entities.CarService;
using CarSpace.Data.Models.Entities.CarShop;
using CarSpace.Data.Models.Entities.Contact;
using CarSpace.Data.Models.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CarSpace.Data;

public class CarSpaceDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public CarSpaceDbContext(DbContextOptions<CarSpaceDbContext> options)
        : base(options)
    {
    }

    public DbSet<CarForumArticle> CarForumArticles { get; set; }
    public DbSet<CarForumComment> CarForumArticleComments { get; set; }
    public DbSet<UserSavedCarForumArticle> UserSavedCarForumArticles { get; set; }
    public DbSet<CarForumBrand> CarForumBrands { get; set; }

    public DbSet<CarServiceListing> CarServiceListings { get; set; }
    public DbSet<UserSavedCarServiceListing> UserSavedCarServicesListings { get; set; } 
    public DbSet<CarServiceCategory> CarServiceCategories { get; set; }

    public DbSet<CarMeetListing> CarMeetListings { get; set; }
    public DbSet<UserSavedCarMeetListing> UserSavedCarMeetListings { get; set; } = null!;
    public DbSet<UserJoinedCarMeetListing> UserJoinedCarMeetListings { get; set; } = null!;

    public DbSet<CarShopListing> CarShopListings { get; set; } = null!;
    public DbSet<UserSavedCarShopListing> UserSavedCarShopListings { get; set; } = null!;
    public DbSet<CarShopBrand> CarShopBrands { get; set; } = null!;

    public DbSet<AboutUs> AboutUs { get; set; }
    public DbSet<ContactUs> ContactUs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
