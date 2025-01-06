using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.DataAccess.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecourseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceKey",
                table: "Resources");

            migrationBuilder.RenameColumn(
                name: "ResourceValue",
                table: "Resources",
                newName: "Key");

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "Resources",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Resources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019409bf-3ae7-7cdf-995b-db4620f2ff5f",
                column: "DateOfBirth",
                value: new DateOnly(2025, 1, 4));

            migrationBuilder.CreateIndex(
                name: "IX_Resources_FileId",
                table: "Resources",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_UploadedFiles_FileId",
                table: "Resources",
                column: "FileId",
                principalTable: "UploadedFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_UploadedFiles_FileId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_FileId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Resources");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "Resources",
                newName: "ResourceValue");

            migrationBuilder.AddColumn<string>(
                name: "ResourceKey",
                table: "Resources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019409bf-3ae7-7cdf-995b-db4620f2ff5f",
                column: "DateOfBirth",
                value: new DateOnly(2025, 1, 1));
        }
    }
}
