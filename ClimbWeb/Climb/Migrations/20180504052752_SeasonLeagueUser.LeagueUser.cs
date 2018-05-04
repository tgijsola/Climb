using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Climb.Migrations
{
    public partial class SeasonLeagueUserLeagueUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeasonLeagueUsers_Leagues_LeagueID",
                table: "SeasonLeagueUsers");

            migrationBuilder.DropIndex(
                name: "IX_SeasonLeagueUsers_LeagueID",
                table: "SeasonLeagueUsers");

            migrationBuilder.DropColumn(
                name: "LeagueID",
                table: "SeasonLeagueUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeagueID",
                table: "SeasonLeagueUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeasonLeagueUsers_LeagueID",
                table: "SeasonLeagueUsers",
                column: "LeagueID");

            migrationBuilder.AddForeignKey(
                name: "FK_SeasonLeagueUsers_Leagues_LeagueID",
                table: "SeasonLeagueUsers",
                column: "LeagueID",
                principalTable: "Leagues",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
