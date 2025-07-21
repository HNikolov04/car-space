using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarSpace.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedInitialCarMeetFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarMeets",
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
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarMeets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarMeets_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CarMeets_CreatorId",
                table: "CarMeets",
                column: "CreatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarMeetParticipants");

            migrationBuilder.DropTable(
                name: "CarMeets");
        }
    }
}
