using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.DataAccess.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBlockedColumnToUserCourseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamTimes",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "CompleteStatus",
                table: "UserCourses");

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "UserCourses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "UserCourses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "UserCourses");

            migrationBuilder.AddColumn<int>(
                name: "ExamTimes",
                table: "UserExams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CompleteStatus",
                table: "UserCourses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "InProgress");
        }
    }
}
