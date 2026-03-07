using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vezeeta_Clone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BloodTypeInPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Blood_Type",
                table: "Patients",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WaitingTimeInMinutes",
                table: "Doctors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Blood_Type",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "WaitingTimeInMinutes",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
