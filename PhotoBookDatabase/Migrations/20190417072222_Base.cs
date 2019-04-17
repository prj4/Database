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
                name: "Pictures",
                columns: table => new
                {
                    PictureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventPin = table.Column<string>(nullable: true),
                    TakerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.PictureId);
                });

            migrationBuilder.CreateTable(
                name: "PictureTakers",
                columns: table => new
                {
                    PictureTakerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    EventPin = table.Column<string>(nullable: true),
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
                        name: "FK_Events_PictureTakers_HostId",
                        column: x => x.HostId,
                        principalTable: "PictureTakers",
                        principalColumn: "PictureTakerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "PictureTakers",
                columns: new[] { "PictureTakerId", "Discriminator", "Name", "Email" },
                values: new object[,]
                {
                    { 1, "Host", "Host1", "Email1@email.com" },
                    { 2, "Host", "Host2", "Email2@email.com" },
                    { 3, "Host", "Host3", "Email3@email.com" },
                    { 4, "Host", "Host5", "Email5@email.com" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Pin", "Description", "EndDate", "HostId", "Location", "Name", "StartDate" },
                values: new object[,]
                {
                    { "1", "Beskrivelse1", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), 1, "Lokation1", "Event1", new DateTime(2019, 4, 17, 9, 22, 22, 202, DateTimeKind.Local).AddTicks(6784) },
                    { "1234", "Beskrivelse4", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), 1, "Lokation4", "Event4", new DateTime(2019, 4, 17, 9, 22, 22, 204, DateTimeKind.Local).AddTicks(8417) },
                    { "2", "Beskrivelse2", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), 2, "Lokation2", "Event2", new DateTime(2019, 4, 17, 9, 22, 22, 204, DateTimeKind.Local).AddTicks(8389) },
                    { "2345", "Beskrivelse5", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), 2, "Lokation5", "Event5", new DateTime(2019, 4, 17, 9, 22, 22, 204, DateTimeKind.Local).AddTicks(8424) },
                    { "3", "Beskrivelse3", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), 3, "Lokation3", "Event3", new DateTime(2019, 4, 17, 9, 22, 22, 204, DateTimeKind.Local).AddTicks(8413) },
                    { "3456", "Beskrivelse6", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), 3, "Lokation6", "Event6", new DateTime(2019, 4, 17, 9, 22, 22, 204, DateTimeKind.Local).AddTicks(8428) }
                });

            migrationBuilder.InsertData(
                table: "PictureTakers",
                columns: new[] { "PictureTakerId", "Discriminator", "Name", "EventPin" },
                values: new object[,]
                {
                    { 5, "Guest", "Guest1", "1" },
                    { 6, "Guest", "Guest2", "2" },
                    { 7, "Guest", "Guest3", "3" }
                });

            migrationBuilder.InsertData(
                table: "Pictures",
                columns: new[] { "PictureId", "EventPin", "TakerId" },
                values: new object[,]
                {
                    { 1, "1", 1 },
                    { 2, "2", 2 },
                    { 3, "3", 3 }
                });

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
                name: "IX_PictureTakers_EventPin",
                table: "PictureTakers",
                column: "EventPin");

            migrationBuilder.CreateIndex(
                name: "IX_PictureTakers_Email",
                table: "PictureTakers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_PictureTakers_TakerId",
                table: "Pictures",
                column: "TakerId",
                principalTable: "PictureTakers",
                principalColumn: "PictureTakerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Events_EventPin",
                table: "Pictures",
                column: "EventPin",
                principalTable: "Events",
                principalColumn: "Pin",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PictureTakers_Events_EventPin",
                table: "PictureTakers",
                column: "EventPin",
                principalTable: "Events",
                principalColumn: "Pin",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_PictureTakers_HostId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "PictureTakers");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
