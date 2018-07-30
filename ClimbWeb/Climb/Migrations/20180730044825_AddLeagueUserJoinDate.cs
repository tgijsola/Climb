using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Climb.Migrations
{
    public partial class AddLeagueUserJoinDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TieBreakerPoints",
                table: "SeasonLeagueUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinDate",
                table: "LeagueUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TieBreakerPoints",
                table: "SeasonLeagueUsers");

            migrationBuilder.DropColumn(
                name: "JoinDate",
                table: "LeagueUsers");
        }
    }
}
