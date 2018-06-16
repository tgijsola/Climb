using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Climb.Migrations
{
    public partial class AddColumnsForRanks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "Sets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Sets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "LeagueUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "LeagueUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "LeagueUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SetCount",
                table: "LeagueUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Leagues",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SetsTillRank",
                table: "Leagues",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Games",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RankSnapshots",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LeagueUserID = table.Column<int>(nullable: false),
                    Rank = table.Column<int>(nullable: false),
                    DeltaRank = table.Column<int>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    DeltaPoints = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankSnapshots", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RankSnapshots_LeagueUsers_LeagueUserID",
                        column: x => x.LeagueUserID,
                        principalTable: "LeagueUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RankSnapshots_LeagueUserID",
                table: "RankSnapshots",
                column: "LeagueUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RankSnapshots");

            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "LeagueUsers");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "LeagueUsers");

            migrationBuilder.DropColumn(
                name: "SetCount",
                table: "LeagueUsers");

            migrationBuilder.DropColumn(
                name: "SetsTillRank",
                table: "Leagues");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "LeagueUsers",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Leagues",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Games",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
