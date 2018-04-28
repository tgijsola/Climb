using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Climb.Migrations
{
    public partial class SeasonLeagueUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "LeagueUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "SeasonLeagueUsers",
                columns: table => new
                {
                    LeagueUserID = table.Column<int>(nullable: false),
                    SeasonID = table.Column<int>(nullable: false),
                    LeagueID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonLeagueUsers", x => new { x.LeagueUserID, x.SeasonID });
                    table.ForeignKey(
                        name: "FK_SeasonLeagueUsers_Leagues_LeagueID",
                        column: x => x.LeagueID,
                        principalTable: "Leagues",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SeasonLeagueUsers_LeagueUsers_LeagueUserID",
                        column: x => x.LeagueUserID,
                        principalTable: "LeagueUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeasonLeagueUsers_Seasons_SeasonID",
                        column: x => x.SeasonID,
                        principalTable: "Seasons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeagueUsers_LeagueID",
                table: "LeagueUsers",
                column: "LeagueID");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueUsers_UserID",
                table: "LeagueUsers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonLeagueUsers_LeagueID",
                table: "SeasonLeagueUsers",
                column: "LeagueID");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonLeagueUsers_SeasonID",
                table: "SeasonLeagueUsers",
                column: "SeasonID");

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueUsers_Leagues_LeagueID",
                table: "LeagueUsers",
                column: "LeagueID",
                principalTable: "Leagues",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueUsers_AspNetUsers_UserID",
                table: "LeagueUsers",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeagueUsers_Leagues_LeagueID",
                table: "LeagueUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_LeagueUsers_AspNetUsers_UserID",
                table: "LeagueUsers");

            migrationBuilder.DropTable(
                name: "SeasonLeagueUsers");

            migrationBuilder.DropIndex(
                name: "IX_LeagueUsers_LeagueID",
                table: "LeagueUsers");

            migrationBuilder.DropIndex(
                name: "IX_LeagueUsers_UserID",
                table: "LeagueUsers");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "LeagueUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
