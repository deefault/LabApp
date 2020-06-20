using Microsoft.EntityFrameworkCore.Migrations;

namespace LabApp.Server.Migrations
{
    public partial class add_IsHidden : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "GroupLessons",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "GroupAssignment",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "GroupLessons");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "GroupAssignment");
        }
    }
}
