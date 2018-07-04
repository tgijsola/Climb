using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Climb.Migrations
{
    public partial class AddSetRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SetRequests",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RequesterID = table.Column<int>(nullable: false),
                    ChallengedID = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetRequests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SetRequests_LeagueUsers_ChallengedID",
                        column: x => x.ChallengedID,
                        principalTable: "LeagueUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SetRequests_LeagueUsers_RequesterID",
                        column: x => x.RequesterID,
                        principalTable: "LeagueUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SetRequests_ChallengedID",
                table: "SetRequests",
                column: "ChallengedID");

            migrationBuilder.CreateIndex(
                name: "IX_SetRequests_RequesterID",
                table: "SetRequests",
                column: "RequesterID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SetRequests");
        }
    }
}
