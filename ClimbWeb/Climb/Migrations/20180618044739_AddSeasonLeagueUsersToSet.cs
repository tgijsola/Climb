using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Climb.Migrations
{
    public partial class AddSeasonLeagueUsersToSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SeasonLeagueUsers",
                table: "SeasonLeagueUsers");

            migrationBuilder.AddColumn<int>(
                name: "SeasonPlayer1ID",
                table: "Sets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeasonPlayer2ID",
                table: "Sets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "SeasonLeagueUsers",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeasonLeagueUsers",
                table: "SeasonLeagueUsers",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_SeasonPlayer1ID",
                table: "Sets",
                column: "SeasonPlayer1ID");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_SeasonPlayer2ID",
                table: "Sets",
                column: "SeasonPlayer2ID");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonLeagueUsers_LeagueUserID",
                table: "SeasonLeagueUsers",
                column: "LeagueUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_SeasonLeagueUsers_SeasonPlayer1ID",
                table: "Sets",
                column: "SeasonPlayer1ID",
                principalTable: "SeasonLeagueUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_SeasonLeagueUsers_SeasonPlayer2ID",
                table: "Sets",
                column: "SeasonPlayer2ID",
                principalTable: "SeasonLeagueUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sets_SeasonLeagueUsers_SeasonPlayer1ID",
                table: "Sets");

            migrationBuilder.DropForeignKey(
                name: "FK_Sets_SeasonLeagueUsers_SeasonPlayer2ID",
                table: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_Sets_SeasonPlayer1ID",
                table: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_Sets_SeasonPlayer2ID",
                table: "Sets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeasonLeagueUsers",
                table: "SeasonLeagueUsers");

            migrationBuilder.DropIndex(
                name: "IX_SeasonLeagueUsers_LeagueUserID",
                table: "SeasonLeagueUsers");

            migrationBuilder.DropColumn(
                name: "SeasonPlayer1ID",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "SeasonPlayer2ID",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "SeasonLeagueUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeasonLeagueUsers",
                table: "SeasonLeagueUsers",
                columns: new[] { "LeagueUserID", "SeasonID" });
        }
    }
}
