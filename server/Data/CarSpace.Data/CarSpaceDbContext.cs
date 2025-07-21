using CarSpace.Data.Models.CarForum;
using CarSpace.Data.Models.Entities.CarForum;
using CarSpace.Data.Models.Entities.CarServices;
using CarSpace.Data.Models.Entities.Meet;
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

    public DbSet<CarServiceListing> CarServiceListings { get; set; }

    public DbSet<CarMeet> CarMeets { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
