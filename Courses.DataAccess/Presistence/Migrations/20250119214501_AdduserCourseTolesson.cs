using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.DataAccess.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AdduserCourseTolesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserCourseId",
                table: "UserLessons",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserLessons_UserCourseId",
                table: "UserLessons",
                column: "UserCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLessons_UserCourses_UserCourseId",
                table: "UserLessons",
                column: "UserCourseId",
                principalTable: "UserCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLessons_UserCourses_UserCourseId",
                table: "UserLessons");

            migrationBuilder.DropIndex(
                name: "IX_UserLessons_UserCourseId",
                table: "UserLessons");

            migrationBuilder.DropColumn(
                name: "UserCourseId",
                table: "UserLessons");
        }
    }
}
