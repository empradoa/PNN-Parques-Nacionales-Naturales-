using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PNN.Web.Migrations
{
    public partial class DB_WArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parks_Locations_LocationId",
                table: "Parks");

            migrationBuilder.DropForeignKey(
                name: "FK_Zones_Locations_LocationId",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Zones_LocationId",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Parks_LocationId",
                table: "Parks");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Parks");

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LocationId = table.Column<int>(nullable: false),
                    ParkId = table.Column<int>(nullable: true),
                    ZoneId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Areas_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Areas_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Areas_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Areas_LocationId",
                table: "Areas",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_ParkId",
                table: "Areas",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_ZoneId",
                table: "Areas",
                column: "ZoneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Zones",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Parks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Zones_LocationId",
                table: "Zones",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Parks_LocationId",
                table: "Parks",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parks_Locations_LocationId",
                table: "Parks",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_Locations_LocationId",
                table: "Zones",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
