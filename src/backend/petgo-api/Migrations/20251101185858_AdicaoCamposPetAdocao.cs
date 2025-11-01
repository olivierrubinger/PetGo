using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace petgo.api.Migrations
{
    /// <inheritdoc />
    public partial class AdicaoCamposPetAdocao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Castrado",
                table: "Produtos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataNascimento",
                table: "Produtos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Especie",
                table: "Produtos",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Porte",
                table: "Produtos",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Raca",
                table: "Produtos",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Saude",
                table: "Produtos",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Vacinado",
                table: "Produtos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Castrado",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Especie",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Porte",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Raca",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Saude",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Vacinado",
                table: "Produtos");
        }
    }
}
