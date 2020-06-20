using Microsoft.EntityFrameworkCore.Migrations;

namespace LabApp.Server.Migrations
{
    public partial class fixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAssignmentAttachment_StudentAssignment_StudentAssign~",
                table: "StudentAssignmentAttachment");

            migrationBuilder.AlterColumn<int>(
                name: "StudentAssignmentId",
                table: "StudentAssignmentAttachment",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "AdditionalScores",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAssignmentAttachment_StudentAssignment_StudentAssign~",
                table: "StudentAssignmentAttachment",
                column: "StudentAssignmentId",
                principalTable: "StudentAssignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAssignmentAttachment_StudentAssignment_StudentAssign~",
                table: "StudentAssignmentAttachment");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "AdditionalScores");

            migrationBuilder.AlterColumn<int>(
                name: "StudentAssignmentId",
                table: "StudentAssignmentAttachment",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAssignmentAttachment_StudentAssignment_StudentAssign~",
                table: "StudentAssignmentAttachment",
                column: "StudentAssignmentId",
                principalTable: "StudentAssignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
