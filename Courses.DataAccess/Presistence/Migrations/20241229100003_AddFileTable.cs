using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Courses.DataAccess.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Courses");

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "Lessons",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.AddColumn<Guid>(
                name: "ThumbnailId",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 15, "permissions", "role:add", "019409cc-7157-71a4-99d5-e295c82679db" },
                    { 16, "permissions", "role:update", "019409cc-7157-71a4-99d5-e295c82679db" },
                    { 17, "permissions", "role:toggle", "019409cc-7157-71a4-99d5-e295c82679db" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019409bf-3ae7-7cdf-995b-db4620f2ff5f",
                column: "DateOfBirth",
                value: new DateOnly(2024, 12, 29));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Lessons");

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
                name: "ThumbnailId",
                table: "Courses");

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

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Lessons",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Courses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019409bf-3ae7-7cdf-995b-db4620f2ff5f",
                column: "DateOfBirth",
                value: new DateOnly(2024, 12, 28));
        }
    }
}
