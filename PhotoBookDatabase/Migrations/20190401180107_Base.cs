using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoBookDatabase.Migrations
{
    public partial class Base : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PictureTakers",
                columns: table => new
                {
                    PictureTakerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    PW = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PictureTakers", x => x.PictureTakerId);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Pin = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Location = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    HostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Pin);
                    table.ForeignKey(
                        name: "FK_Events_PictureTakers_HostId",
                        column: x => x.HostId,
                        principalTable: "PictureTakers",
                        principalColumn: "PictureTakerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventGuests",
                columns: table => new
                {
                    Guest_Id = table.Column<int>(nullable: false),
                    Event_Pin = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventGuests", x => new { x.Event_Pin, x.Guest_Id });
                    table.ForeignKey(
                        name: "FK_EventGuests_Events_Event_Pin",
                        column: x => x.Event_Pin,
                        principalTable: "Events",
                        principalColumn: "Pin",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventGuests_PictureTakers_Guest_Id",
                        column: x => x.Guest_Id,
                        principalTable: "PictureTakers",
                        principalColumn: "PictureTakerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    PictureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    URL = table.Column<string>(nullable: true),
                    EventPin = table.Column<int>(nullable: false),
                    Taker = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.PictureId);
                    table.ForeignKey(
                        name: "FK_Pictures_Events_EventPin",
                        column: x => x.EventPin,
                        principalTable: "Events",
                        principalColumn: "Pin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pictures_PictureTakers_Taker",
                        column: x => x.Taker,
                        principalTable: "PictureTakers",
                        principalColumn: "PictureTakerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventGuests_Guest_Id",
                table: "EventGuests",
                column: "Guest_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Events_HostId",
                table: "Events",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_EventPin",
                table: "Pictures",
                column: "EventPin");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_Taker",
                table: "Pictures",
                column: "Taker");

            migrationBuilder.CreateIndex(
                name: "IX_PictureTakers_Email",
                table: "PictureTakers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PictureTakers_Username",
                table: "PictureTakers",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventGuests");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "PictureTakers");
        }
    }
}
