using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarSpace.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEntityNamesAndRemoveTrackedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedCarForumArticles_AspNetUsers_UserId",
                table: "UserSavedCarForumArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedCarForumArticles_CarForumArticles_CarForumArticleId",
                table: "UserSavedCarForumArticles");

            migrationBuilder.DropTable(
                name: "UserCarMeetParticipants");

            migrationBuilder.DropTable(
                name: "UserSavedCarMeets");

            migrationBuilder.DropTable(
                name: "UserSavedCarsAndSuvsListings");

            migrationBuilder.DropTable(
                name: "UserSavedCarServices");

            migrationBuilder.DropTable(
                name: "CarMeets");

            migrationBuilder.DropTable(
                name: "CarsAndSuvsListings");

            migrationBuilder.DropTable(
                name: "CarCarsAndSuvsListingBrands");

            migrationBuilder.DeleteData(
                table: "AboutUs",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6d034953-dc38-4d45-ea21-08dd2099696a"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("7487f894-3a49-4bce-ea22-08dd2099696a"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "CarForumBrands",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CarForumBrands",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CarForumBrands",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CarServiceCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CarServiceCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CarServiceCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CarServiceCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CarServiceCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ContactUs",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7487f894-3a49-4bce-ea22-08dd2099696a"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "ContactUs",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "AboutUs",
                newName: "UpdatedAt");

            migrationBuilder.CreateTable(
                name: "CarMeetListings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MeetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarMeetListings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarMeetListings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarShopBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarShopBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSavedCarServicesListings",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarServiceListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSavedCarServicesListings", x => new { x.UserId, x.CarServiceListingId });
                    table.ForeignKey(
                        name: "FK_UserSavedCarServicesListings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSavedCarServicesListings_CarServiceListings_CarServiceListingId",
                        column: x => x.CarServiceListingId,
                        principalTable: "CarServiceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserJoinedCarMeetListings",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarMeetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJoinedCarMeetListings", x => new { x.UserId, x.CarMeetId });
                    table.ForeignKey(
                        name: "FK_UserJoinedCarMeetListings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserJoinedCarMeetListings_CarMeetListings_CarMeetId",
                        column: x => x.CarMeetId,
                        principalTable: "CarMeetListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSavedCarMeetListings",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarMeetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSavedCarMeetListings", x => new { x.UserId, x.CarMeetId });
                    table.ForeignKey(
                        name: "FK_UserSavedCarMeetListings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSavedCarMeetListings_CarMeetListings_CarMeetId",
                        column: x => x.CarMeetId,
                        principalTable: "CarMeetListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarShopListings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false),
                    Horsepower = table.Column<int>(type: "int", nullable: false),
                    Transmission = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FuelType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EuroStandard = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Doors = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CarBrandId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarShopListings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarShopListings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarShopListings_CarShopBrands_CarBrandId",
                        column: x => x.CarBrandId,
                        principalTable: "CarShopBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSavedCarShopListings",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarShopListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSavedCarShopListings", x => new { x.UserId, x.CarShopListingId });
                    table.ForeignKey(
                        name: "FK_UserSavedCarShopListings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSavedCarShopListings_CarShopListings_CarShopListingId",
                        column: x => x.CarShopListingId,
                        principalTable: "CarShopListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarMeetListings_UserId",
                table: "CarMeetListings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CarShopListings_CarBrandId",
                table: "CarShopListings",
                column: "CarBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_CarShopListings_UserId",
                table: "CarShopListings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJoinedCarMeetListings_CarMeetId",
                table: "UserJoinedCarMeetListings",
                column: "CarMeetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedCarMeetListings_CarMeetId",
                table: "UserSavedCarMeetListings",
                column: "CarMeetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedCarServicesListings_CarServiceListingId",
                table: "UserSavedCarServicesListings",
                column: "CarServiceListingId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedCarShopListings_CarShopListingId",
                table: "UserSavedCarShopListings",
                column: "CarShopListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedCarForumArticles_AspNetUsers_UserId",
                table: "UserSavedCarForumArticles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedCarForumArticles_CarForumArticles_CarForumArticleId",
                table: "UserSavedCarForumArticles",
                column: "CarForumArticleId",
                principalTable: "CarForumArticles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedCarForumArticles_AspNetUsers_UserId",
                table: "UserSavedCarForumArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedCarForumArticles_CarForumArticles_CarForumArticleId",
                table: "UserSavedCarForumArticles");

            migrationBuilder.DropTable(
                name: "UserJoinedCarMeetListings");

            migrationBuilder.DropTable(
                name: "UserSavedCarMeetListings");

            migrationBuilder.DropTable(
                name: "UserSavedCarServicesListings");

            migrationBuilder.DropTable(
                name: "UserSavedCarShopListings");

            migrationBuilder.DropTable(
                name: "CarMeetListings");

            migrationBuilder.DropTable(
                name: "CarShopListings");

            migrationBuilder.DropTable(
                name: "CarShopBrands");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "ContactUs",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "AboutUs",
                newName: "LastUpdated");

            migrationBuilder.CreateTable(
                name: "CarCarsAndSuvsListingBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarCarsAndSuvsListingBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarMeets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    MeetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarMeets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarMeets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSavedCarServices",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarServiceListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSavedCarServices", x => new { x.UserId, x.CarServiceListingId });
                    table.ForeignKey(
                        name: "FK_UserSavedCarServices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSavedCarServices_CarServiceListings_CarServiceListingId",
                        column: x => x.CarServiceListingId,
                        principalTable: "CarServiceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CarsAndSuvsListings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarBrandId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Doors = table.Column<int>(type: "int", nullable: false),
                    EuroStandard = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FuelType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Horsepower = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Transmission = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarsAndSuvsListings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarsAndSuvsListings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarsAndSuvsListings_CarCarsAndSuvsListingBrands_CarBrandId",
                        column: x => x.CarBrandId,
                        principalTable: "CarCarsAndSuvsListingBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserCarMeetParticipants",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarMeetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCarMeetParticipants", x => new { x.UserId, x.CarMeetId });
                    table.ForeignKey(
                        name: "FK_UserCarMeetParticipants_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCarMeetParticipants_CarMeets_CarMeetId",
                        column: x => x.CarMeetId,
                        principalTable: "CarMeets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSavedCarMeets",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarMeetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSavedCarMeets", x => new { x.UserId, x.CarMeetId });
                    table.ForeignKey(
                        name: "FK_UserSavedCarMeets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSavedCarMeets_CarMeets_CarMeetId",
                        column: x => x.CarMeetId,
                        principalTable: "CarMeets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSavedCarsAndSuvsListings",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarsAndSuvsListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSavedCarsAndSuvsListings", x => new { x.UserId, x.CarsAndSuvsListingId });
                    table.ForeignKey(
                        name: "FK_UserSavedCarsAndSuvsListings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSavedCarsAndSuvsListings_CarsAndSuvsListings_CarsAndSuvsListingId",
                        column: x => x.CarsAndSuvsListingId,
                        principalTable: "CarsAndSuvsListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AboutUs",
                columns: new[] { "Id", "LastUpdated", "Message", "Title" },
                values: new object[] { new Guid("11111111-0000-0000-0000-000000000001"), new DateTime(2025, 8, 3, 23, 30, 37, 310, DateTimeKind.Local).AddTicks(5220), "CarSpace is a platform dedicated to car enthusiasts and drivers alike. Whether you're looking to buy, sell, service, or meet up — we're here to support your car journey.", "About CarSpace" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("6d034953-dc38-4d45-ea21-08dd2099696a"), null, "User", "USER" },
                    { new Guid("7487f894-3a49-4bce-ea22-08dd2099696a"), null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "ImageUrl", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), 0, "a3a53ee2-84f8-4017-833a-685cfab63ff5", "admin@carspace.com", false, "/user-content/profile-pictures/CarSpaceUserDefaultWhiteModePfpDefault.png", false, false, null, null, null, "AQAAAAIAAYagAAAAEG0xK0jycgXGLWL9/TIPDn0FniPOJ81avDzGHEAj+XxuBv37kJWuwVSuNYnvnyvKew==", null, false, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "CarCarsAndSuvsListingBrands",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "BMW" },
                    { 2, "Audi" },
                    { 3, "Mercedes" }
                });

            migrationBuilder.InsertData(
                table: "CarForumBrands",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "BMW" },
                    { 2, "Audi" },
                    { 3, "Mercedes" }
                });

            migrationBuilder.InsertData(
                table: "CarServiceCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Repair" },
                    { 2, "Washing" },
                    { 3, "Detailing" },
                    { 4, "Tire Services" },
                    { 5, "Oil Change" }
                });

            migrationBuilder.InsertData(
                table: "ContactUs",
                columns: new[] { "Id", "Email", "LastUpdated", "Message", "Phone", "Title" },
                values: new object[] { new Guid("11111111-0000-0000-0000-000000000002"), "support@carspace.com", new DateTime(2025, 8, 3, 23, 30, 37, 315, DateTimeKind.Local).AddTicks(3972), "Reach out with any questions or feedback — we’d love to hear from you.", "0888888888", "Contact Us" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("7487f894-3a49-4bce-ea22-08dd2099696a"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.CreateIndex(
                name: "IX_CarMeets_UserId",
                table: "CarMeets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CarsAndSuvsListings_CarBrandId",
                table: "CarsAndSuvsListings",
                column: "CarBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_CarsAndSuvsListings_UserId",
                table: "CarsAndSuvsListings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCarMeetParticipants_CarMeetId",
                table: "UserCarMeetParticipants",
                column: "CarMeetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedCarMeets_CarMeetId",
                table: "UserSavedCarMeets",
                column: "CarMeetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedCarsAndSuvsListings_CarsAndSuvsListingId",
                table: "UserSavedCarsAndSuvsListings",
                column: "CarsAndSuvsListingId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedCarServices_CarServiceListingId",
                table: "UserSavedCarServices",
                column: "CarServiceListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedCarForumArticles_AspNetUsers_UserId",
                table: "UserSavedCarForumArticles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedCarForumArticles_CarForumArticles_CarForumArticleId",
                table: "UserSavedCarForumArticles",
                column: "CarForumArticleId",
                principalTable: "CarForumArticles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
