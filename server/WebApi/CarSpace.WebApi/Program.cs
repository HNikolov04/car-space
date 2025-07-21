using CarSpace.Data.Models.Entities.User;
using CarSpace.Services.Core.Interfaces;
using CarSpace.Services.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CarSpace.Data;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Data.Repositories;

namespace CarSpace.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<CarSpaceDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services
        .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 3;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<CarSpaceDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddScoped<ICarForumRepository, CarForumRepository>();
        builder.Services.AddScoped<ICarForumService, CarForumService>();

        builder.Services.AddScoped<ICarServiceListingRepository, CarServiceListingRepository>();
        builder.Services.AddScoped<ICarServiceListingService, CarServiceListingService>();

        builder.Services.AddScoped<ICarMeetRepository, CarMeetRepository>();
        builder.Services.AddScoped<ICarMeetService, CarMeetService>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors("AllowAll");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
