using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.DataAccess.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameOrderToOrderIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Order",
                table: "Modules",
                newName: "OrderIndex");

            migrationBuilder.RenameColumn(
                name: "Index",
                table: "ModuleItems",
                newName: "OrderIndex");

            migrationBuilder.RenameIndex(
                name: "IX_ModuleItems_ModuleId_Index",
                table: "ModuleItems",
                newName: "IX_ModuleItems_ModuleId_OrderIndex");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderIndex",
                table: "Modules",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "OrderIndex",
                table: "ModuleItems",
                newName: "Index");

            migrationBuilder.RenameIndex(
                name: "IX_ModuleItems_ModuleId_OrderIndex",
                table: "ModuleItems",
                newName: "IX_ModuleItems_ModuleId_Index");
        }
    }
}
