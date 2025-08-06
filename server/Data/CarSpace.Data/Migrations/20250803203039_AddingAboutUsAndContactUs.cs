using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarSpace.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingAboutUsAndContactUs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutUs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactUs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AboutUs",
                columns: new[] { "Id", "LastUpdated", "Message", "Title" },
                values: new object[] { new Guid("11111111-0000-0000-0000-000000000001"), new DateTime(2025, 8, 3, 23, 30, 37, 310, DateTimeKind.Local).AddTicks(5220), "CarSpace is a platform dedicated to car enthusiasts and drivers alike. Whether you're looking to buy, sell, service, or meet up — we're here to support your car journey.", "About CarSpace" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a3a53ee2-84f8-4017-833a-685cfab63ff5", "AQAAAAIAAYagAAAAEG0xK0jycgXGLWL9/TIPDn0FniPOJ81avDzGHEAj+XxuBv37kJWuwVSuNYnvnyvKew==" });

            migrationBuilder.InsertData(
                table: "ContactUs",
                columns: new[] { "Id", "Email", "LastUpdated", "Message", "Phone", "Title" },
                values: new object[] { new Guid("11111111-0000-0000-0000-000000000002"), "support@carspace.com", new DateTime(2025, 8, 3, 23, 30, 37, 315, DateTimeKind.Local).AddTicks(3972), "Reach out with any questions or feedback — we’d love to hear from you.", "0888888888", "Contact Us" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutUs");

            migrationBuilder.DropTable(
                name: "ContactUs");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f292f53c-b02a-46b9-84c7-8e9064381b7a", "AQAAAAIAAYagAAAAEIcnDXjy/hky/Uy8blryjYYq45Yu/AcYrXYbp5E6UW9Sv3caP4Gx9M+nfrWbhlPQ2g==" });
        }
    }
}
