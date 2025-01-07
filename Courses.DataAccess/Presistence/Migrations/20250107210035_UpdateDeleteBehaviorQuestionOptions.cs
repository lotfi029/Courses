using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.DataAccess.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehaviorQuestionOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Optinos_OptionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Optinos_Questions_QuestionId",
                table: "Optinos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Optinos",
                table: "Optinos");

            migrationBuilder.RenameTable(
                name: "Optinos",
                newName: "Options");

            migrationBuilder.RenameIndex(
                name: "IX_Optinos_Text_QuestionId",
                table: "Options",
                newName: "IX_Options_Text_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Optinos_QuestionId",
                table: "Options",
                newName: "IX_Options_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Options",
                table: "Options",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Options_OptionId",
                table: "Answers",
                column: "OptionId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Options_OptionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Options",
                table: "Options");

            migrationBuilder.RenameTable(
                name: "Options",
                newName: "Optinos");

            migrationBuilder.RenameIndex(
                name: "IX_Options_Text_QuestionId",
                table: "Optinos",
                newName: "IX_Optinos_Text_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Options_QuestionId",
                table: "Optinos",
                newName: "IX_Optinos_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Optinos",
                table: "Optinos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Optinos_OptionId",
                table: "Answers",
                column: "OptionId",
                principalTable: "Optinos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Optinos_Questions_QuestionId",
                table: "Optinos",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
