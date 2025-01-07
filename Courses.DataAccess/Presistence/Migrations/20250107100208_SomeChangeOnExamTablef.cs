using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.DataAccess.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class SomeChangeOnExamTablef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestion_Exams_ExamsId",
                table: "ExamQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestion_Questions_QuestionsId",
                table: "ExamQuestion");

            migrationBuilder.RenameColumn(
                name: "QuestionsId",
                table: "ExamQuestion",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "ExamsId",
                table: "ExamQuestion",
                newName: "ExamId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamQuestion_QuestionsId",
                table: "ExamQuestion",
                newName: "IX_ExamQuestion_QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamQuestion_Exams_ExamId",
                table: "ExamQuestion",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamQuestion_Questions_QuestionId",
                table: "ExamQuestion",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestion_Exams_ExamId",
                table: "ExamQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestion_Questions_QuestionId",
                table: "ExamQuestion");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "ExamQuestion",
                newName: "QuestionsId");

            migrationBuilder.RenameColumn(
                name: "ExamId",
                table: "ExamQuestion",
                newName: "ExamsId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamQuestion_QuestionId",
                table: "ExamQuestion",
                newName: "IX_ExamQuestion_QuestionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamQuestion_Exams_ExamsId",
                table: "ExamQuestion",
                column: "ExamsId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamQuestion_Questions_QuestionsId",
                table: "ExamQuestion",
                column: "QuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
