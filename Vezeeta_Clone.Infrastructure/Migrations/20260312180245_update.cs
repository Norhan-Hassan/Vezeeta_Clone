using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vezeeta_Clone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clinics_Locations_ClinicLocationID",
                table: "Clinics");

            migrationBuilder.RenameColumn(
                name: "ClinicLocationID",
                table: "Clinics",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Clinics_ClinicLocationID",
                table: "Clinics",
                newName: "IX_Clinics_LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_Locations_LocationId",
                table: "Clinics",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clinics_Locations_LocationId",
                table: "Clinics");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Clinics",
                newName: "ClinicLocationID");

            migrationBuilder.RenameIndex(
                name: "IX_Clinics_LocationId",
                table: "Clinics",
                newName: "IX_Clinics_ClinicLocationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_Locations_ClinicLocationID",
                table: "Clinics",
                column: "ClinicLocationID",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
