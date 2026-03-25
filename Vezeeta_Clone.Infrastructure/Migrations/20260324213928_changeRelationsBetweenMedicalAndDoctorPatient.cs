using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vezeeta_Clone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeRelationsBetweenMedicalAndDoctorPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Doctors_DoctorId",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Patients_PatientId",
                table: "MedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_MedicalRecords_DoctorId",
                table: "MedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_MedicalRecords_PatientId",
                table: "MedicalRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorPatients",
                table: "DoctorPatients");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "MedicalRecords");

            migrationBuilder.AddColumn<int>(
                name: "DoctorPatientId",
                table: "MedicalRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "EPrescriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "DoctorPatients",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "TotalVisits",
                table: "DoctorPatients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Diagnoses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorPatients",
                table: "DoctorPatients",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_DoctorPatientId",
                table: "MedicalRecords",
                column: "DoctorPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorPatients_DoctorId_PatientId",
                table: "DoctorPatients",
                columns: new[] { "DoctorId", "PatientId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_DoctorPatients_DoctorPatientId",
                table: "MedicalRecords",
                column: "DoctorPatientId",
                principalTable: "DoctorPatients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_DoctorPatients_DoctorPatientId",
                table: "MedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_MedicalRecords_DoctorPatientId",
                table: "MedicalRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorPatients",
                table: "DoctorPatients");

            migrationBuilder.DropIndex(
                name: "IX_DoctorPatients_DoctorId_PatientId",
                table: "DoctorPatients");

            migrationBuilder.DropColumn(
                name: "DoctorPatientId",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "EPrescriptions");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "DoctorPatients");

            migrationBuilder.DropColumn(
                name: "TotalVisits",
                table: "DoctorPatients");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Diagnoses");

            migrationBuilder.AddColumn<string>(
                name: "DoctorId",
                table: "MedicalRecords",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientId",
                table: "MedicalRecords",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorPatients",
                table: "DoctorPatients",
                columns: new[] { "DoctorId", "PatientId" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_DoctorId",
                table: "MedicalRecords",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_PatientId",
                table: "MedicalRecords",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Doctors_DoctorId",
                table: "MedicalRecords",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "AppUserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Patients_PatientId",
                table: "MedicalRecords",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "AppUserID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
