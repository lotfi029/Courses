using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.DataAccess.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDisableColumnToModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisable",
                table: "Modules",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisable",
                table: "Modules");
        }
    }
}
