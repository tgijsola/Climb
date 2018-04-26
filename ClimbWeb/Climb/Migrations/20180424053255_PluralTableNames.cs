using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Climb.Migrations
{
    public partial class PluralTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_League_Games_GameID",
                table: "League");

            migrationBuilder.DropForeignKey(
                name: "FK_Season_League_LeagueID",
                table: "Season");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Season",
                table: "Season");

            migrationBuilder.DropPrimaryKey(
                name: "PK_League",
                table: "League");

            migrationBuilder.RenameTable(
                name: "Season",
                newName: "Seasons");

            migrationBuilder.RenameTable(
                name: "League",
                newName: "Leagues");

            migrationBuilder.RenameIndex(
                name: "IX_Season_LeagueID",
                table: "Seasons",
                newName: "IX_Seasons_LeagueID");

            migrationBuilder.RenameIndex(
                name: "IX_League_GameID",
                table: "Leagues",
                newName: "IX_Leagues_GameID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seasons",
                table: "Seasons",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Leagues",
                table: "Leagues",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Leagues_Games_GameID",
                table: "Leagues",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seasons_Leagues_LeagueID",
                table: "Seasons",
                column: "LeagueID",
                principalTable: "Leagues",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leagues_Games_GameID",
                table: "Leagues");

            migrationBuilder.DropForeignKey(
                name: "FK_Seasons_Leagues_LeagueID",
                table: "Seasons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seasons",
                table: "Seasons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Leagues",
                table: "Leagues");

            migrationBuilder.RenameTable(
                name: "Seasons",
                newName: "Season");

            migrationBuilder.RenameTable(
                name: "Leagues",
                newName: "League");

            migrationBuilder.RenameIndex(
                name: "IX_Seasons_LeagueID",
                table: "Season",
                newName: "IX_Season_LeagueID");

            migrationBuilder.RenameIndex(
                name: "IX_Leagues_GameID",
                table: "League",
                newName: "IX_League_GameID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Season",
                table: "Season",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_League",
                table: "League",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_League_Games_GameID",
                table: "League",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Season_League_LeagueID",
                table: "Season",
                column: "LeagueID",
                principalTable: "League",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
