using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.DataAccess.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_UploadedFiles_ThumbnailId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_ThumbnailId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ThumbnailId",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Courses");

            migrationBuilder.AddColumn<Guid>(
                name: "ThumbnailId",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ThumbnailId",
                table: "Courses",
                column: "ThumbnailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_UploadedFiles_ThumbnailId",
                table: "Courses",
                column: "ThumbnailId",
                principalTable: "UploadedFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
