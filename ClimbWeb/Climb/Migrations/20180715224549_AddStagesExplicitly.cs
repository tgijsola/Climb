using Microsoft.EntityFrameworkCore.Migrations;

namespace Climb.Migrations
{
    public partial class AddStagesExplicitly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Stage_StageID",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Stage_Games_GameID",
                table: "Stage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stage",
                table: "Stage");

            migrationBuilder.RenameTable(
                name: "Stage",
                newName: "Stages");

            migrationBuilder.RenameIndex(
                name: "IX_Stage_GameID",
                table: "Stages",
                newName: "IX_Stages_GameID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stages",
                table: "Stages",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Stages_StageID",
                table: "Matches",
                column: "StageID",
                principalTable: "Stages",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_Games_GameID",
                table: "Stages",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Stages_StageID",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Stages_Games_GameID",
                table: "Stages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stages",
                table: "Stages");

            migrationBuilder.RenameTable(
                name: "Stages",
                newName: "Stage");

            migrationBuilder.RenameIndex(
                name: "IX_Stages_GameID",
                table: "Stage",
                newName: "IX_Stage_GameID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stage",
                table: "Stage",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Stage_StageID",
                table: "Matches",
                column: "StageID",
                principalTable: "Stage",
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
    }
}
