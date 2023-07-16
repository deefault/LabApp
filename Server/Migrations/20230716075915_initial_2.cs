using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class initial_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventOutbox_DateTime",
                table: "EventOutbox");

            migrationBuilder.DropIndex(
                name: "IX_EventInbox_DateTime",
                table: "EventInbox");

            migrationBuilder.CreateIndex(
                name: "IX_EventOutbox_DateTime",
                table: "EventOutbox",
                column: "DateTime",
                descending: new bool[0],
                filter: "\"DateDelete\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EventInbox_DateTime",
                table: "EventInbox",
                column: "DateTime",
                descending: new bool[0],
                filter: "\"DateDelete\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventOutbox_DateTime",
                table: "EventOutbox");

            migrationBuilder.DropIndex(
                name: "IX_EventInbox_DateTime",
                table: "EventInbox");

            migrationBuilder.CreateIndex(
                name: "IX_EventOutbox_DateTime",
                table: "EventOutbox",
                column: "DateTime",
                descending: new bool[0],
                filter: "DateDelete IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EventInbox_DateTime",
                table: "EventInbox",
                column: "DateTime",
                descending: new bool[0],
                filter: "DateDelete IS NOT NULL");
        }
    }
}
