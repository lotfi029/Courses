using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.DataAccess.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProgressColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Progress",
                table: "UserCourses",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Progress",
                table: "UserCourses");
        }
    }
}
