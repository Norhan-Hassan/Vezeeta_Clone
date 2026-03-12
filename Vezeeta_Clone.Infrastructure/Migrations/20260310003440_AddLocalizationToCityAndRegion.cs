using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vezeeta_Clone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalizationToCityAndRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Regions",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "City",
                newName: "NameEn");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Regions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "City",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "City");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Regions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "City",
                newName: "Name");
        }
    }
}
