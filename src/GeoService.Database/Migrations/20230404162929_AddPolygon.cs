using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoService.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddPolygon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PolygonId",
                table: "PointDouble",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Polygons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polygons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointDouble_PolygonId",
                table: "PointDouble",
                column: "PolygonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointDouble_Polygons_PolygonId",
                table: "PointDouble",
                column: "PolygonId",
                principalTable: "Polygons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointDouble_Polygons_PolygonId",
                table: "PointDouble");

            migrationBuilder.DropTable(
                name: "Polygons");

            migrationBuilder.DropIndex(
                name: "IX_PointDouble_PolygonId",
                table: "PointDouble");

            migrationBuilder.DropColumn(
                name: "PolygonId",
                table: "PointDouble");
        }
    }
}
