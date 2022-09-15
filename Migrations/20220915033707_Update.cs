using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mutantes.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abilities",
                table: "Mutants");

            migrationBuilder.AddColumn<string>(
                name: "Abilities_one",
                table: "Mutants",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Abilities_tree",
                table: "Mutants",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Abilities_two",
                table: "Mutants",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abilities_one",
                table: "Mutants");

            migrationBuilder.DropColumn(
                name: "Abilities_tree",
                table: "Mutants");

            migrationBuilder.DropColumn(
                name: "Abilities_two",
                table: "Mutants");

            migrationBuilder.AddColumn<string>(
                name: "Abilities",
                table: "Mutants",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
