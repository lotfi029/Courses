using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUserModuleItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExams_AspNetUsers_UserId",
                table: "UserExams");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExams_ModuleItems_ModuleItemId",
                table: "UserExams");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLessons_AspNetUsers_UserId",
                table: "UserLessons");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLessons_ModuleItems_ModuleItemId",
                table: "UserLessons");

            migrationBuilder.DropIndex(
                name: "IX_UserLessons_ModuleItemId",
                table: "UserLessons");

            migrationBuilder.DropIndex(
                name: "IX_UserLessons_UserId",
                table: "UserLessons");

            migrationBuilder.DropIndex(
                name: "IX_UserExams_ModuleItemId",
                table: "UserExams");

            migrationBuilder.DropIndex(
                name: "IX_UserExams_UserId",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "UserLessons");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "UserLessons");

            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "UserLessons");

            migrationBuilder.DropColumn(
                name: "ModuleItemId",
                table: "UserLessons");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "UserLessons");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserLessons");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "ModuleItemId",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserExams");

            migrationBuilder.CreateTable(
                name: "UserModuleItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModuleItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModuleItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserModuleItems_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserModuleItems_ModuleItems_ModuleItemId",
                        column: x => x.ModuleItemId,
                        principalTable: "ModuleItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserModuleItems_ModuleItemId",
                table: "UserModuleItems",
                column: "ModuleItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModuleItems_UserId",
                table: "UserModuleItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExams_UserModuleItems_Id",
                table: "UserExams",
                column: "Id",
                principalTable: "UserModuleItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLessons_UserModuleItems_Id",
                table: "UserLessons",
                column: "Id",
                principalTable: "UserModuleItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExams_UserModuleItems_Id",
                table: "UserExams");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLessons_UserModuleItems_Id",
                table: "UserLessons");

            migrationBuilder.DropTable(
                name: "UserModuleItems");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "UserLessons",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "UserLessons",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "UserLessons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModuleItemId",
                table: "UserLessons",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "UserLessons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserLessons",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "UserExams",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "UserExams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "UserExams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModuleItemId",
                table: "UserExams",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "UserExams",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserExams",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserLessons_ModuleItemId",
                table: "UserLessons",
                column: "ModuleItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLessons_UserId",
                table: "UserLessons",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExams_ModuleItemId",
                table: "UserExams",
                column: "ModuleItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExams_UserId",
                table: "UserExams",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExams_AspNetUsers_UserId",
                table: "UserExams",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExams_ModuleItems_ModuleItemId",
                table: "UserExams",
                column: "ModuleItemId",
                principalTable: "ModuleItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLessons_AspNetUsers_UserId",
                table: "UserLessons",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLessons_ModuleItems_ModuleItemId",
                table: "UserLessons",
                column: "ModuleItemId",
                principalTable: "ModuleItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
