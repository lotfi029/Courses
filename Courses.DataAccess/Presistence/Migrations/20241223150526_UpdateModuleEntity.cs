using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.DataAccess.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModuleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Modules_Title_CourseId",
                table: "Modules",
                columns: ["Title", "CourseId"],
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Modules_Title_CourseId",
                table: "Modules");
        }
    }
}
