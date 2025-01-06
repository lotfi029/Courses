using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.DataAccess.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class UploadFileconfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_UploadedFile_ThumbnailId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_UploadedFile_FileId",
                table: "Lessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UploadedFile",
                table: "UploadedFile");

            migrationBuilder.RenameTable(
                name: "UploadedFile",
                newName: "UploadedFiles");

            migrationBuilder.AlterColumn<string>(
                name: "StoredFileName",
                table: "UploadedFiles",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "UploadedFiles",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FileExtension",
                table: "UploadedFiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "UploadedFiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UploadedFiles",
                table: "UploadedFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_UploadedFiles_ThumbnailId",
                table: "Courses",
                column: "ThumbnailId",
                principalTable: "UploadedFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_UploadedFiles_FileId",
                table: "Lessons",
                column: "FileId",
                principalTable: "UploadedFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_UploadedFiles_ThumbnailId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_UploadedFiles_FileId",
                table: "Lessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UploadedFiles",
                table: "UploadedFiles");

            migrationBuilder.RenameTable(
                name: "UploadedFiles",
                newName: "UploadedFile");

            migrationBuilder.AlterColumn<string>(
                name: "StoredFileName",
                table: "UploadedFile",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "UploadedFile",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "FileExtension",
                table: "UploadedFile",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "UploadedFile",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UploadedFile",
                table: "UploadedFile",
                column: "Id");

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
    }
}
