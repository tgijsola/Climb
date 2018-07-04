using Microsoft.EntityFrameworkCore.Migrations;

namespace Climb.Migrations
{
    public partial class AddLeagueToSetRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeagueID",
                table: "SetRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SetRequests_LeagueID",
                table: "SetRequests",
                column: "LeagueID");

            migrationBuilder.AddForeignKey(
                name: "FK_SetRequests_Leagues_LeagueID",
                table: "SetRequests",
                column: "LeagueID",
                principalTable: "Leagues",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SetRequests_Leagues_LeagueID",
                table: "SetRequests");

            migrationBuilder.DropIndex(
                name: "IX_SetRequests_LeagueID",
                table: "SetRequests");

            migrationBuilder.DropColumn(
                name: "LeagueID",
                table: "SetRequests");
        }
    }
}
