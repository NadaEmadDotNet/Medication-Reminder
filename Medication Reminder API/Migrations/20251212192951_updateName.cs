using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medication_Reminder_API.Migrations
{
    /// <inheritdoc />
    public partial class updateName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorPatient_Doctors_DoctorId",
                table: "DoctorPatient");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorPatient_Patients_PatientId",
                table: "DoctorPatient");

            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Patients_PatientID",
                table: "Medications");

            migrationBuilder.DropIndex(
                name: "IX_Medications_PatientID",
                table: "Medications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorPatient",
                table: "DoctorPatient");

            migrationBuilder.DropColumn(
                name: "PatientID",
                table: "Medications");

            migrationBuilder.RenameTable(
                name: "DoctorPatient",
                newName: "DoctorPatients");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Medications",
                newName: "DurationInDays");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorPatient_PatientId",
                table: "DoctorPatients",
                newName: "IX_DoctorPatients_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorPatients",
                table: "DoctorPatients",
                columns: new[] { "DoctorId", "PatientId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorPatients_Doctors_DoctorId",
                table: "DoctorPatients",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorPatients_Patients_PatientId",
                table: "DoctorPatients",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorPatients_Doctors_DoctorId",
                table: "DoctorPatients");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorPatients_Patients_PatientId",
                table: "DoctorPatients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorPatients",
                table: "DoctorPatients");

            migrationBuilder.RenameTable(
                name: "DoctorPatients",
                newName: "DoctorPatient");

            migrationBuilder.RenameColumn(
                name: "DurationInDays",
                table: "Medications",
                newName: "Duration");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorPatients_PatientId",
                table: "DoctorPatient",
                newName: "IX_DoctorPatient_PatientId");

            migrationBuilder.AddColumn<int>(
                name: "PatientID",
                table: "Medications",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorPatient",
                table: "DoctorPatient",
                columns: new[] { "DoctorId", "PatientId" });

            migrationBuilder.CreateIndex(
                name: "IX_Medications_PatientID",
                table: "Medications",
                column: "PatientID");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorPatient_Doctors_DoctorId",
                table: "DoctorPatient",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorPatient_Patients_PatientId",
                table: "DoctorPatient",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_Patients_PatientID",
                table: "Medications",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID");
        }
    }
}
