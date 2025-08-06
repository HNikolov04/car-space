using CarSpace.Data;
using CarSpace.Data.Models.Entities.About;
using CarSpace.Data.Models.Entities.CarForum;
using CarSpace.Data.Models.Entities.CarService;
using CarSpace.Data.Models.Entities.CarShop;
using CarSpace.Data.Models.Entities.Contact;
using CarSpace.Data.Models.Entities.User;
using CarSpace.Data.Seeding.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class Seeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CarSpaceDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var isSeeded = await context.Roles.AnyAsync()
              || await context.Users.AnyAsync()
              || await context.CarForumBrands.AnyAsync()
              || await context.CarServiceCategories.AnyAsync()
              || await context.CarShopBrands.AnyAsync()
              || await context.AboutUs.AnyAsync()
              || await context.ContactUs.AnyAsync();

        if (isSeeded)
        {
            return;
        }

        await SeedRolesAsync(roleManager);
        await SeedAdminUserAsync(userManager);
        await SeedCarForumBrandsAsync(context);
        await SeedCarServiceCategoriesAsync(context);
        await SeedCarShopBrandsAsync(context);
        await SeedAboutUsAsync(context);
        await SeedContactUsAsync(context);

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
    {
        var roles = new[]
        {
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("74BF7894-3AA9-4BEC-EA22-8B0D229B966A"),
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            },
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("60034953-DC83-4DB5-944E-8B0D229B966A"), 
                Name = "User",
                NormalizedName = "USER"
            }
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role.Name))
            {
                await roleManager.CreateAsync(role);
            }
        }
    }

    private async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
    {
        var adminEmail = "admin@carspace.com";
        var existingUser = await userManager.FindByEmailAsync(adminEmail);

        if (existingUser != null) return;

        var admin = new ApplicationUser(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            adminEmail,
            "admin",
            "/user-content/profile-pictures/CarSpaceUserDefaultWhiteModePfpDefault.png"
        );

        var result = await userManager.CreateAsync(admin, "Admin123!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Administrator");
        }
    }

    private async Task SeedCarForumBrandsAsync(CarSpaceDbContext context)
    {
        if (await context.CarForumBrands.AnyAsync()) return;

        var brands = new List<CarForumBrand>
        {
            new() { Name = "BMW" },
            new() { Name = "Audi" },
            new() { Name = "Mercedes" }
        };

        await context.CarForumBrands.AddRangeAsync(brands);
    }

    private async Task SeedCarServiceCategoriesAsync(CarSpaceDbContext context)
    {
        if (await context.CarServiceCategories.AnyAsync()) return;

        var categories = new List<CarServiceCategory>
        {
            new() { Name = "Repair" },
            new() { Name = "Washing" },
            new() { Name = "Detailing" },
            new() { Name = "Tire Services" },
            new() { Name = "Oil Change" }
        };

        await context.CarServiceCategories.AddRangeAsync(categories);
    }

    private async Task SeedCarShopBrandsAsync(CarSpaceDbContext context)
    {
        if (await context.CarShopBrands.AnyAsync()) return;

        var brands = new List<CarShopBrand>
        {
            new() { Name = "BMW" },
            new() { Name = "Audi" },
            new() { Name = "Mercedes" }
        };

        await context.CarShopBrands.AddRangeAsync(brands);
    }

    private async Task SeedAboutUsAsync(CarSpaceDbContext context)
    {
        if (await context.AboutUs.AnyAsync()) return;

        var about = new AboutUs
        {
            Id = Guid.Parse("11111111-0000-0000-0000-000000000001"),
            Title = "About CarSpace",
            Message = "CarSpace is a platform dedicated to car enthusiasts and drivers alike. Whether you're looking to buy, sell, service, or meet up – we're here to support your car journey."
        };

        await context.AboutUs.AddAsync(about);
    }

    private async Task SeedContactUsAsync(CarSpaceDbContext context)
    {
        if (await context.ContactUs.AnyAsync()) return;

        var contact = new ContactUs
        {
            Id = Guid.Parse("11111111-0000-0000-0000-000000000021"),
            Title = "Contact Us",
            Email = "support@carspace.com",
            Message = "Reach out with any questions or feedback – we’d love to hear from you.",
            Phone = "0888888888"
        };

        await context.ContactUs.AddAsync(contact);
    }
}
