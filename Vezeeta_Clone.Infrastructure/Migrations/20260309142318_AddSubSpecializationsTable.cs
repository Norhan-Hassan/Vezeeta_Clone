using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vezeeta_Clone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubSpecializationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Specializations");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Specializations",
                newName: "NameEn");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Specializations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SubSpecializations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecializationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubSpecializations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SubSpecializations_Specializations_SpecializationId",
                        column: x => x.SpecializationId,
                        principalTable: "Specializations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoctorSubSpecializations",
                columns: table => new
                {
                    DoctorsAppUserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubSpecializationsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorSubSpecializations", x => new { x.DoctorsAppUserID, x.SubSpecializationsID });
                    table.ForeignKey(
                        name: "FK_DoctorSubSpecializations_Doctors_DoctorsAppUserID",
                        column: x => x.DoctorsAppUserID,
                        principalTable: "Doctors",
                        principalColumn: "AppUserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoctorSubSpecializations_SubSpecializations_SubSpecializationsID",
                        column: x => x.SubSpecializationsID,
                        principalTable: "SubSpecializations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSubSpecializations_SubSpecializationsID",
                table: "DoctorSubSpecializations",
                column: "SubSpecializationsID");

            migrationBuilder.CreateIndex(
                name: "IX_SubSpecializations_SpecializationId",
                table: "SubSpecializations",
                column: "SpecializationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorSubSpecializations");

            migrationBuilder.DropTable(
                name: "SubSpecializations");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Specializations");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Specializations",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Specializations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
