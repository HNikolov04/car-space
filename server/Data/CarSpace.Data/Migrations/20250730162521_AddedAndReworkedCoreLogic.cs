using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarSpace.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedAndReworkedCoreLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarMeets_AspNetUsers_CreatorId",
                table: "CarMeets");

            migrationBuilder.DropTable(
                name: "CarMeetParticipants");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "CarServiceListings");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "CarsAndSuvsListings");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "CarForumArticles");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "CarMeets",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CarMeets_CreatorId",
                table: "CarMeets",
                newName: "IX_CarMeets_UserId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "CarServiceListings",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CarServiceListings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "CarServiceListings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CarServiceListings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "CarsAndSuvsListings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CarBrandId",
                table: "CarsAndSuvsListings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CarsAndSuvsListings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CarMeets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CarBrandId",
                table: "CarForumArticles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CarForumArticles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                name: "CarForumBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarForumBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarServiceCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarServiceCategories", x => x.Id);
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
                name: "UserSavedCarForumArticles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarForumArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSavedCarForumArticles", x => new { x.UserId, x.CarForumArticleId });
                    table.ForeignKey(
                        name: "FK_UserSavedCarForumArticles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSavedCarForumArticles_CarForumArticles_CarForumArticleId",
                        column: x => x.CarForumArticleId,
                        principalTable: "CarForumArticles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateIndex(
                name: "IX_CarServiceListings_CategoryId",
                table: "CarServiceListings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CarsAndSuvsListings_CarBrandId",
                table: "CarsAndSuvsListings",
                column: "CarBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_CarForumArticles_CarBrandId",
                table: "CarForumArticles",
                column: "CarBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCarMeetParticipants_CarMeetId",
                table: "UserCarMeetParticipants",
                column: "CarMeetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedCarForumArticles_CarForumArticleId",
                table: "UserSavedCarForumArticles",
                column: "CarForumArticleId");

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
                name: "FK_CarForumArticles_CarForumBrands_CarBrandId",
                table: "CarForumArticles",
                column: "CarBrandId",
                principalTable: "CarForumBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CarMeets_AspNetUsers_UserId",
                table: "CarMeets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarsAndSuvsListings_CarCarsAndSuvsListingBrands_CarBrandId",
                table: "CarsAndSuvsListings",
                column: "CarBrandId",
                principalTable: "CarCarsAndSuvsListingBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CarServiceListings_CarServiceCategories_CategoryId",
                table: "CarServiceListings",
                column: "CategoryId",
                principalTable: "CarServiceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarForumArticles_CarForumBrands_CarBrandId",
                table: "CarForumArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_CarMeets_AspNetUsers_UserId",
                table: "CarMeets");

            migrationBuilder.DropForeignKey(
                name: "FK_CarsAndSuvsListings_CarCarsAndSuvsListingBrands_CarBrandId",
                table: "CarsAndSuvsListings");

            migrationBuilder.DropForeignKey(
                name: "FK_CarServiceListings_CarServiceCategories_CategoryId",
                table: "CarServiceListings");

            migrationBuilder.DropTable(
                name: "CarCarsAndSuvsListingBrands");

            migrationBuilder.DropTable(
                name: "CarForumBrands");

            migrationBuilder.DropTable(
                name: "CarServiceCategories");

            migrationBuilder.DropTable(
                name: "UserCarMeetParticipants");

            migrationBuilder.DropTable(
                name: "UserSavedCarForumArticles");

            migrationBuilder.DropTable(
                name: "UserSavedCarMeets");

            migrationBuilder.DropTable(
                name: "UserSavedCarsAndSuvsListings");

            migrationBuilder.DropTable(
                name: "UserSavedCarServices");

            migrationBuilder.DropIndex(
                name: "IX_CarServiceListings_CategoryId",
                table: "CarServiceListings");

            migrationBuilder.DropIndex(
                name: "IX_CarsAndSuvsListings_CarBrandId",
                table: "CarsAndSuvsListings");

            migrationBuilder.DropIndex(
                name: "IX_CarForumArticles_CarBrandId",
                table: "CarForumArticles");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "CarServiceListings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CarServiceListings");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "CarsAndSuvsListings");

            migrationBuilder.DropColumn(
                name: "CarBrandId",
                table: "CarsAndSuvsListings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CarsAndSuvsListings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CarMeets");

            migrationBuilder.DropColumn(
                name: "CarBrandId",
                table: "CarForumArticles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CarForumArticles");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CarMeets",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_CarMeets_UserId",
                table: "CarMeets",
                newName: "IX_CarMeets_CreatorId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "CarServiceListings",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CarServiceListings",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "CarServiceListings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "CarsAndSuvsListings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "CarForumArticles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CarMeetParticipants",
                columns: table => new
                {
                    JoinedMeetsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipantsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarMeetParticipants", x => new { x.JoinedMeetsId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_CarMeetParticipants_AspNetUsers_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarMeetParticipants_CarMeets_JoinedMeetsId",
                        column: x => x.JoinedMeetsId,
                        principalTable: "CarMeets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarMeetParticipants_ParticipantsId",
                table: "CarMeetParticipants",
                column: "ParticipantsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarMeets_AspNetUsers_CreatorId",
                table: "CarMeets",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
