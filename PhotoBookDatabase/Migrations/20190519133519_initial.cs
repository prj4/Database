using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoBookDatabase.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hosts",
                columns: table => new
                {
                    HostId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hosts", x => x.HostId);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Pin = table.Column<string>(nullable: false),
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
                        name: "FK_Events_Hosts_HostId",
                        column: x => x.HostId,
                        principalTable: "Hosts",
                        principalColumn: "HostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    GuestId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    EventPin = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.GuestId);
                    table.ForeignKey(
                        name: "FK_Guests_Events_EventPin",
                        column: x => x.EventPin,
                        principalTable: "Events",
                        principalColumn: "Pin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    PictureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventPin = table.Column<string>(nullable: false),
                    GuestId = table.Column<int>(nullable: true),
                    HostId = table.Column<int>(nullable: true)
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
                        name: "FK_Pictures_Guests_GuestId",
                        column: x => x.GuestId,
                        principalTable: "Guests",
                        principalColumn: "GuestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pictures_Hosts_HostId",
                        column: x => x.HostId,
                        principalTable: "Hosts",
                        principalColumn: "HostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Hosts",
                columns: new[] { "HostId", "Email", "Name" },
                values: new object[,]
                {
                    { 1, "Email1@email.com", "Host1" },
                    { 2, "Email2@email.com", "Host2" },
                    { 3, "Email3@email.com", "Host3" },
                    { 4, "Email5@email.com", "Host5" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Pin", "Description", "EndDate", "HostId", "Location", "Name", "StartDate" },
                values: new object[,]
                {
                    { "1", "Beskrivelse1", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), 1, "Lokation1", "Event1", new DateTime(2019, 5, 19, 15, 35, 18, 286, DateTimeKind.Local).AddTicks(1901) },
                    { "1234", "Beskrivelse4", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), 1, "Lokation4", "Event4", new DateTime(2019, 5, 19, 15, 35, 18, 289, DateTimeKind.Local).AddTicks(2) },
                    { "2", "Beskrivelse2", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), 2, "Lokation2", "Event2", new DateTime(2019, 5, 19, 15, 35, 18, 288, DateTimeKind.Local).AddTicks(9978) },
                    { "2345", "Beskrivelse5", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), 2, "Lokation5", "Event5", new DateTime(2019, 5, 19, 15, 35, 18, 289, DateTimeKind.Local).AddTicks(4) },
                    { "3", "Beskrivelse3", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), 3, "Lokation3", "Event3", new DateTime(2019, 5, 19, 15, 35, 18, 288, DateTimeKind.Local).AddTicks(9999) },
                    { "3456", "Beskrivelse6", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), 3, "Lokation6", "Event6", new DateTime(2019, 5, 19, 15, 35, 18, 289, DateTimeKind.Local).AddTicks(10) }
                });

            migrationBuilder.InsertData(
                table: "Guests",
                columns: new[] { "GuestId", "EventPin", "Name" },
                values: new object[,]
                {
                    { 1, "1", "Guest1" },
                    { 2, "2", "Guest2" },
                    { 3, "3", "Guest3" }
                });

            migrationBuilder.InsertData(
                table: "Pictures",
                columns: new[] { "PictureId", "EventPin", "GuestId", "HostId" },
                values: new object[,]
                {
                    { 1, "1", null, 1 },
                    { 2, "2", null, 2 },
                    { 3, "3", null, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_HostId",
                table: "Events",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_EventPin",
                table: "Guests",
                column: "EventPin");

            migrationBuilder.CreateIndex(
                name: "IX_Hosts_Email",
                table: "Hosts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_EventPin",
                table: "Pictures",
                column: "EventPin");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_GuestId",
                table: "Pictures",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_HostId",
                table: "Pictures",
                column: "HostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "Guests");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Hosts");
        }
    }
}
