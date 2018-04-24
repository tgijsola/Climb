using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Climb.Migrations
{
    public partial class League : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Character_Game_GameID",
                table: "Character");

            migrationBuilder.DropForeignKey(
                name: "FK_Stage_Game_GameID",
                table: "Stage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Game",
                table: "Game");

            migrationBuilder.RenameTable(
                name: "Game",
                newName: "Games");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                table: "Games",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Leagues_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_GameID",
                table: "Leagues",
                column: "GameID");

            migrationBuilder.AddForeignKey(
                name: "FK_Character_Games_GameID",
                table: "Character",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stage_Games_GameID",
                table: "Stage",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Character_Games_GameID",
                table: "Character");

            migrationBuilder.DropForeignKey(
                name: "FK_Stage_Games_GameID",
                table: "Stage");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                table: "Games");

            migrationBuilder.RenameTable(
                name: "Games",
                newName: "Game");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Game",
                table: "Game",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Character_Game_GameID",
                table: "Character",
                column: "GameID",
                principalTable: "Game",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stage_Game_GameID",
                table: "Stage",
                column: "GameID",
                principalTable: "Game",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
