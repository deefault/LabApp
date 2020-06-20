using Microsoft.EntityFrameworkCore.Migrations;

namespace LabApp.Server.Migrations
{
    public partial class add_groupId_on_assignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "StudentAssignment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentAssignment_GroupId",
                table: "StudentAssignment",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAssignment_Groups_GroupId",
                table: "StudentAssignment",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAssignment_Groups_GroupId",
                table: "StudentAssignment");

            migrationBuilder.DropIndex(
                name: "IX_StudentAssignment_GroupId",
                table: "StudentAssignment");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "StudentAssignment");
        }
    }
}
