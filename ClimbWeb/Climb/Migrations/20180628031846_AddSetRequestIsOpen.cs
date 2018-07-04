using Microsoft.EntityFrameworkCore.Migrations;

namespace Climb.Migrations
{
    public partial class AddSetRequestIsOpen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "SetRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SetID",
                table: "SetRequests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SetRequests_SetID",
                table: "SetRequests",
                column: "SetID");

            migrationBuilder.AddForeignKey(
                name: "FK_SetRequests_Sets_SetID",
                table: "SetRequests",
                column: "SetID",
                principalTable: "Sets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SetRequests_Sets_SetID",
                table: "SetRequests");

            migrationBuilder.DropIndex(
                name: "IX_SetRequests_SetID",
                table: "SetRequests");

            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "SetRequests");

            migrationBuilder.DropColumn(
                name: "SetID",
                table: "SetRequests");
        }
    }
}
