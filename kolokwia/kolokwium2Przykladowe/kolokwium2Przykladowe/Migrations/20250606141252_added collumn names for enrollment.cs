using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kolokwium2Przykladowe.Migrations
{
    /// <inheritdoc />
    public partial class addedcollumnnamesforenrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Course_Id",
                table: "Enrollment",
                newName: "Course_ID");

            migrationBuilder.RenameColumn(
                name: "Student_Id",
                table: "Enrollment",
                newName: "Student_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Course_ID",
                table: "Enrollment",
                newName: "Course_Id");

            migrationBuilder.RenameColumn(
                name: "Student_ID",
                table: "Enrollment",
                newName: "Student_Id");
        }
    }
}
