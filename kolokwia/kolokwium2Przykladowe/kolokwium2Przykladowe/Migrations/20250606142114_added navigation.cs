using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kolokwium2Przykladowe.Migrations
{
    /// <inheritdoc />
    public partial class addednavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_Course_ID",
                table: "Enrollment",
                column: "Course_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Course_Course_ID",
                table: "Enrollment",
                column: "Course_ID",
                principalTable: "Course",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Student_Student_ID",
                table: "Enrollment",
                column: "Student_ID",
                principalTable: "Student",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Course_Course_ID",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Student_Student_ID",
                table: "Enrollment");

            migrationBuilder.DropIndex(
                name: "IX_Enrollment_Course_ID",
                table: "Enrollment");
        }
    }
}
