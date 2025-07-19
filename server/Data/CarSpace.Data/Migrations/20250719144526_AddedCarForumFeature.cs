using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarSpace.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCarForumFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarForumArticles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarForumArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarForumArticles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarForumArticleComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarForumArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarForumArticleComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarForumArticleComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarForumArticleComments_CarForumArticles_CarForumArticleId",
                        column: x => x.CarForumArticleId,
                        principalTable: "CarForumArticles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarForumArticleComments_CarForumArticleId",
                table: "CarForumArticleComments",
                column: "CarForumArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_CarForumArticleComments_UserId",
                table: "CarForumArticleComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CarForumArticles_UserId",
                table: "CarForumArticles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarForumArticleComments");

            migrationBuilder.DropTable(
                name: "CarForumArticles");
        }
    }
}
