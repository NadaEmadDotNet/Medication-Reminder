using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medication_Reminder_API.Migrations
{
    /// <inheritdoc />
    public partial class patientname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "DoseLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "DoseLogs");
        }
    }
}
