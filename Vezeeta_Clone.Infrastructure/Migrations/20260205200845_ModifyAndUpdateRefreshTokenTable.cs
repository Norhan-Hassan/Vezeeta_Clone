using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vezeeta_Clone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyAndUpdateRefreshTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "UserRefreshTokens",
                newName: "RefreshToken");

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "UserRefreshTokens",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "UserRefreshTokens");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "UserRefreshTokens",
                newName: "Token");
        }
    }
}
