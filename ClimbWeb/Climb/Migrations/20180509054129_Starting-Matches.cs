using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Climb.Migrations
{
    public partial class StartingMatches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    SetID = table.Column<int>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    Player1Score = table.Column<int>(nullable: false),
                    Player2Score = table.Column<int>(nullable: false),
                    StageID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => new { x.SetID, x.Index });
                    table.ForeignKey(
                        name: "FK_Matches_Sets_SetID",
                        column: x => x.SetID,
                        principalTable: "Sets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Stage_StageID",
                        column: x => x.StageID,
                        principalTable: "Stage",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatchCharacters",
                columns: table => new
                {
                    MatchID = table.Column<int>(nullable: false),
                    CharacterID = table.Column<int>(nullable: false),
                    LeagueUserID = table.Column<int>(nullable: false),
                    MatchIndex = table.Column<int>(nullable: true),
                    MatchSetID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchCharacters", x => new { x.MatchID, x.CharacterID, x.LeagueUserID });
                    table.ForeignKey(
                        name: "FK_MatchCharacters_Character_CharacterID",
                        column: x => x.CharacterID,
                        principalTable: "Character",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchCharacters_LeagueUsers_LeagueUserID",
                        column: x => x.LeagueUserID,
                        principalTable: "LeagueUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchCharacters_Matches_MatchSetID_MatchIndex",
                        columns: x => new { x.MatchSetID, x.MatchIndex },
                        principalTable: "Matches",
                        principalColumns: new[] { "SetID", "Index" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchCharacters_CharacterID",
                table: "MatchCharacters",
                column: "CharacterID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchCharacters_LeagueUserID",
                table: "MatchCharacters",
                column: "LeagueUserID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchCharacters_MatchSetID_MatchIndex",
                table: "MatchCharacters",
                columns: new[] { "MatchSetID", "MatchIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_StageID",
                table: "Matches",
                column: "StageID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchCharacters");

            migrationBuilder.DropTable(
                name: "Matches");
        }
    }
}
