using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.DataAccess.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonAndExamToModuleItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ModuleItems_ModuleId",
                table: "ModuleItems");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "ModuleItems",
                newName: "Index");

            migrationBuilder.AddColumn<Guid>(
                name: "GuidItemId",
                table: "ModuleItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntItemId",
                table: "ModuleItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModuleItems_ModuleId_Index",
                table: "ModuleItems",
                columns: new[] { "ModuleId", "Index" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ModuleItems_ModuleId_Index",
                table: "ModuleItems");

            migrationBuilder.DropColumn(
                name: "GuidItemId",
                table: "ModuleItems");

            migrationBuilder.DropColumn(
                name: "IntItemId",
                table: "ModuleItems");

            migrationBuilder.RenameColumn(
                name: "Index",
                table: "ModuleItems",
                newName: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleItems_ModuleId",
                table: "ModuleItems",
                column: "ModuleId");
        }
    }
}
