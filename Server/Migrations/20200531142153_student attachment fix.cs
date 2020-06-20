using Microsoft.EntityFrameworkCore.Migrations;

namespace LabApp.Server.Migrations
{
    public partial class studentattachmentfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAssignmentAttachment_StudentAssignment_StudentAssign~",
                table: "StudentAssignmentAttachment");

            migrationBuilder.DropIndex(
                name: "IX_StudentAssignmentAttachment_StudentAssignmentId",
                table: "StudentAssignmentAttachment");

            migrationBuilder.DropColumn(
                name: "StudentAssignmentId",
                table: "StudentAssignmentAttachment");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAssignmentAttachment_AssignmentId",
                table: "StudentAssignmentAttachment",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAssignmentAttachment_StudentAssignment_AssignmentId",
                table: "StudentAssignmentAttachment",
                column: "AssignmentId",
                principalTable: "StudentAssignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAssignmentAttachment_StudentAssignment_AssignmentId",
                table: "StudentAssignmentAttachment");

            migrationBuilder.DropIndex(
                name: "IX_StudentAssignmentAttachment_AssignmentId",
                table: "StudentAssignmentAttachment");

            migrationBuilder.AddColumn<int>(
                name: "StudentAssignmentId",
                table: "StudentAssignmentAttachment",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentAssignmentAttachment_StudentAssignmentId",
                table: "StudentAssignmentAttachment",
                column: "StudentAssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAssignmentAttachment_StudentAssignment_StudentAssign~",
                table: "StudentAssignmentAttachment",
                column: "StudentAssignmentId",
                principalTable: "StudentAssignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
