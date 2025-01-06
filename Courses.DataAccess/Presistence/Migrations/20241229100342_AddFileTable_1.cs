using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.DataAccess.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFileTable_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File_ContentType",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "File_FileExtension",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "File_FileName",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "File_Id",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "File_StoredFileName",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Thumbnail_ContentType",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Thumbnail_FileExtension",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Thumbnail_FileName",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Thumbnail_Id",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Thumbnail_StoredFileName",
                table: "Courses");

            migrationBuilder.CreateTable(
                name: "UploadedFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoredFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFile", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_FileId",
                table: "Lessons",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ThumbnailId",
                table: "Courses",
                column: "ThumbnailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_UploadedFile_ThumbnailId",
                table: "Courses",
                column: "ThumbnailId",
                principalTable: "UploadedFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_UploadedFile_FileId",
                table: "Lessons",
                column: "FileId",
                principalTable: "UploadedFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_UploadedFile_ThumbnailId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_UploadedFile_FileId",
                table: "Lessons");

            migrationBuilder.DropTable(
                name: "UploadedFile");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_FileId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Courses_ThumbnailId",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "File_ContentType",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "File_FileExtension",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "File_FileName",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "File_Id",
                table: "Lessons",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "File_StoredFileName",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail_ContentType",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail_FileExtension",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail_FileName",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "Thumbnail_Id",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail_StoredFileName",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
