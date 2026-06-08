using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vezeeta_Clone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class onlineMeeting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnlineMeeting_Appointments_AppointmentId",
                table: "OnlineMeeting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OnlineMeeting",
                table: "OnlineMeeting");

            migrationBuilder.RenameTable(
                name: "OnlineMeeting",
                newName: "OnlineMeetings");

            migrationBuilder.RenameIndex(
                name: "IX_OnlineMeeting_AppointmentId",
                table: "OnlineMeetings",
                newName: "IX_OnlineMeetings_AppointmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OnlineMeetings",
                table: "OnlineMeetings",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineMeetings_Appointments_AppointmentId",
                table: "OnlineMeetings",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnlineMeetings_Appointments_AppointmentId",
                table: "OnlineMeetings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OnlineMeetings",
                table: "OnlineMeetings");

            migrationBuilder.RenameTable(
                name: "OnlineMeetings",
                newName: "OnlineMeeting");

            migrationBuilder.RenameIndex(
                name: "IX_OnlineMeetings_AppointmentId",
                table: "OnlineMeeting",
                newName: "IX_OnlineMeeting_AppointmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OnlineMeeting",
                table: "OnlineMeeting",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineMeeting_Appointments_AppointmentId",
                table: "OnlineMeeting",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
