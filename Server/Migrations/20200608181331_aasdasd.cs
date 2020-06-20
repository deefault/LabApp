using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabApp.Server.Migrations
{
    public partial class aasdasd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "ConversationId",
                table: "Messages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignmentId",
                table: "Conversations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "AdditionalScores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_AssignmentId",
                table: "Conversations",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_StudentAssignment_AssignmentId",
                table: "Conversations",
                column: "AssignmentId",
                principalTable: "StudentAssignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_StudentAssignment_AssignmentId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_AssignmentId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "AdditionalScores");

            migrationBuilder.AlterColumn<int>(
                name: "ConversationId",
                table: "Messages",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
