using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Climb.Migrations
{
    public partial class Season : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leagues_Games_GameID",
                table: "Leagues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Leagues",
                table: "Leagues");

            migrationBuilder.RenameTable(
                name: "Leagues",
                newName: "League");

            migrationBuilder.RenameIndex(
                name: "IX_Leagues_GameID",
                table: "League",
                newName: "IX_League_GameID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_League",
                table: "League",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Season",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    LeagueID = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Season", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Season_League_LeagueID",
                        column: x => x.LeagueID,
                        principalTable: "League",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Season_LeagueID",
                table: "Season",
                column: "LeagueID");

            migrationBuilder.AddForeignKey(
                name: "FK_League_Games_GameID",
                table: "League",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_League_Games_GameID",
                table: "League");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.DropPrimaryKey(
                name: "PK_League",
                table: "League");

            migrationBuilder.RenameTable(
                name: "League",
                newName: "Leagues");

            migrationBuilder.RenameIndex(
                name: "IX_League_GameID",
                table: "Leagues",
                newName: "IX_Leagues_GameID");

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
        }
    }
}
