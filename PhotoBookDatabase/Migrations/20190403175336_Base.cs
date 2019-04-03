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
                    Name = table.Column<string>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
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
                    GuestId = table.Column<int>(nullable: false),
                    EventPin = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventGuests", x => new { x.EventPin, x.GuestId });
                    table.ForeignKey(
                        name: "FK_EventGuests_Events_EventPin",
                        column: x => x.EventPin,
                        principalTable: "Events",
                        principalColumn: "Pin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventGuests_PictureTakers_GuestId",
                        column: x => x.GuestId,
                        principalTable: "PictureTakers",
                        principalColumn: "PictureTakerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    PictureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    URL = table.Column<string>(nullable: false),
                    EventPin = table.Column<int>(nullable: false),
                    TakerId = table.Column<int>(nullable: false)
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
                        name: "FK_Pictures_PictureTakers_TakerId",
                        column: x => x.TakerId,
                        principalTable: "PictureTakers",
                        principalColumn: "PictureTakerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "PictureTakers",
                columns: new[] { "PictureTakerId", "Discriminator", "Name" },
                values: new object[,]
                {
                    { 4, "Guest", "Guest1" },
                    { 5, "Guest", "Guest2" },
                    { 6, "Guest", "Guest3" }
                });

            migrationBuilder.InsertData(
                table: "PictureTakers",
                columns: new[] { "PictureTakerId", "Discriminator", "Name", "Email" },
                values: new object[,]
                {
                    { 1, "Host", "Host1", "Email1@email.com" },
                    { 2, "Host", "Host2", "Email2@email.com" },
                    { 3, "Host", "Host3", "Email3@email.com" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Pin", "Description", "EndDate", "HostId", "Location", "Name", "StartDate" },
                values: new object[] { 1, "Beskrivelse1", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), 1, "Lokation1", "Event1", new DateTime(2019, 4, 3, 19, 53, 35, 685, DateTimeKind.Local).AddTicks(9322) });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Pin", "Description", "EndDate", "HostId", "Location", "Name", "StartDate" },
                values: new object[] { 2, "Beskrivelse2", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), 2, "Lokation2", "Event2", new DateTime(2019, 4, 3, 19, 53, 35, 692, DateTimeKind.Local).AddTicks(4993) });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Pin", "Description", "EndDate", "HostId", "Location", "Name", "StartDate" },
                values: new object[] { 3, "Beskrivelse3", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), 3, "Lokation3", "Event3", new DateTime(2019, 4, 3, 19, 53, 35, 692, DateTimeKind.Local).AddTicks(5014) });

            migrationBuilder.InsertData(
                table: "EventGuests",
                columns: new[] { "EventPin", "GuestId" },
                values: new object[,]
                {
                    { 1, 4 },
                    { 2, 5 },
                    { 3, 6 }
                });

            migrationBuilder.InsertData(
                table: "Pictures",
                columns: new[] { "PictureId", "EventPin", "TakerId", "URL" },
                values: new object[,]
                {
                    { 1, 1, 1, "wwwroot/Images/1.png" },
                    { 2, 2, 2, "wwwroot/Images/2.png" },
                    { 3, 3, 3, "wwwroot/Images/3.png" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventGuests_GuestId",
                table: "EventGuests",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_HostId",
                table: "Events",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_EventPin",
                table: "Pictures",
                column: "EventPin");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_TakerId",
                table: "Pictures",
                column: "TakerId");

            migrationBuilder.CreateIndex(
                name: "IX_PictureTakers_Email",
                table: "PictureTakers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
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
