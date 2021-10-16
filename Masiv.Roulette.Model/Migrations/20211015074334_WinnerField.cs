using Microsoft.EntityFrameworkCore.Migrations;

namespace Masiv.Roulette.Model.Migrations
{
    public partial class WinnerField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Roulettes_RouletteId",
                table: "Bets");

            migrationBuilder.AddColumn<int>(
                name: "Winner",
                table: "Roulettes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RouletteId",
                table: "Bets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Roulettes_RouletteId",
                table: "Bets",
                column: "RouletteId",
                principalTable: "Roulettes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Roulettes_RouletteId",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "Winner",
                table: "Roulettes");

            migrationBuilder.AlterColumn<int>(
                name: "RouletteId",
                table: "Bets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Roulettes_RouletteId",
                table: "Bets",
                column: "RouletteId",
                principalTable: "Roulettes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
