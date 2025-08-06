using CarSpace.Data;
using CarSpace.Data.Models.Entities.User;
using CarSpace.Data.Repositories;
using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Data.Seeding.Interfaces;
using CarSpace.Services.Core.Interfaces;
using CarSpace.Services.Core.Services;
using CarSpace.Services.Core.Services.Interfaces;
using CarSpace.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<CarSpaceDbContext>()
            .AddDefaultTokenProviders();

        var jwtKey = builder.Configuration["Jwt:Key"];
        var jwtIssuer = builder.Configuration["Jwt:Issuer"];
        var jwtAudience = builder.Configuration["Jwt:Audience"];

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
            };
        });

        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' followed by your JWT token"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IImageService, ImageService>();
        builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
        builder.Services.AddScoped<ICarForumArticleRepository, CarForumArticleRepository>();
        builder.Services.AddScoped<ICarForumArticleService, CarForumArticleService>();
        builder.Services.AddScoped<ICarForumCommentRepository, CarForumCommentRepository>();
        builder.Services.AddScoped<ICarForumCommentService, CarForumCommentService>();
        builder.Services.AddScoped<ICarForumBrandRepository, CarForumBrandRepository>();
        builder.Services.AddScoped<ICarForumBrandService, CarForumBrandService>();
        builder.Services.AddScoped<ICarServiceListingRepository, CarServiceListingRepository>();
        builder.Services.AddScoped<ICarServiceListingService, CarServiceListingService>();
        builder.Services.AddScoped<ICarServiceCategoryRepository, CarServiceCategoryRepository>();
        builder.Services.AddScoped<ICarServiceCategoryService, CarServiceCategoryService>();
        builder.Services.AddScoped<ICarMeetListingRepository, CarMeetListingRepository>();
        builder.Services.AddScoped<ICarMeetListingService, CarMeetListingService>();
        builder.Services.AddScoped<ICarShopListingRepository, CarShopListingRepository>();
        builder.Services.AddScoped<ICarShopListingService, CarShopListingService>();
        builder.Services.AddScoped<ICarShopBrandRepository, CarShopBrandRepository>();
        builder.Services.AddScoped<ICarShopBrandService, CarShopBrandService>();
        builder.Services.AddScoped<IAboutUsRepository, AboutUsRepository>();
        builder.Services.AddScoped<IContactUsRepository, ContactUsRepository>();
        builder.Services.AddScoped<IAboutUsService, AboutUsService>();
        builder.Services.AddScoped<IContactUsService, ContactUsService>();

        builder.Services.AddTransient<ISeeder, Seeder>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
            seeder.SeedAsync(scope.ServiceProvider);
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.UseCors("AllowAll");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}