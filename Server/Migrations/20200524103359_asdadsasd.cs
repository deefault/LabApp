using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabApp.Server.Migrations
{
    public partial class asdadsasd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Completed",
                table: "StudentAssignment",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStatusChange",
                table: "StudentAssignment",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Submitted",
                table: "StudentAssignment",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Assignments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_TeacherId",
                table: "Assignments",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Users_TeacherId",
                table: "Assignments",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Users_TeacherId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_TeacherId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "LastStatusChange",
                table: "StudentAssignment");

            migrationBuilder.DropColumn(
                name: "Submitted",
                table: "StudentAssignment");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Assignments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Completed",
                table: "StudentAssignment",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);
        }
    }
}
