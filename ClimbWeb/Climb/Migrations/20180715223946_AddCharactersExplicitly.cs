using Microsoft.EntityFrameworkCore.Migrations;

namespace Climb.Migrations
{
    public partial class AddCharactersExplicitly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Character_Games_GameID",
                table: "Character");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchCharacters_Character_CharacterID",
                table: "MatchCharacters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Character",
                table: "Character");

            migrationBuilder.RenameTable(
                name: "Character",
                newName: "Characters");

            migrationBuilder.RenameIndex(
                name: "IX_Character_GameID",
                table: "Characters",
                newName: "IX_Characters_GameID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Characters",
                table: "Characters",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Games_GameID",
                table: "Characters",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchCharacters_Characters_CharacterID",
                table: "MatchCharacters",
                column: "CharacterID",
                principalTable: "Characters",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Games_GameID",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchCharacters_Characters_CharacterID",
                table: "MatchCharacters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Characters",
                table: "Characters");

            migrationBuilder.RenameTable(
                name: "Characters",
                newName: "Character");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_GameID",
                table: "Character",
                newName: "IX_Character_GameID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Character",
                table: "Character",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Character_Games_GameID",
                table: "Character",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchCharacters_Character_CharacterID",
                table: "MatchCharacters",
                column: "CharacterID",
                principalTable: "Character",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
