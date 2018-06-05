using Microsoft.EntityFrameworkCore.Migrations;

namespace Climb.Migrations
{
    public partial class AddLeagueAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminID",
                table: "Leagues",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_AdminID",
                table: "Leagues",
                column: "AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK_Leagues_AspNetUsers_AdminID",
                table: "Leagues",
                column: "AdminID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leagues_AspNetUsers_AdminID",
                table: "Leagues");

            migrationBuilder.DropIndex(
                name: "IX_Leagues_AdminID",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "AdminID",
                table: "Leagues");
        }
    }
}
