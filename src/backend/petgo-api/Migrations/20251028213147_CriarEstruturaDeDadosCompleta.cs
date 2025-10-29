using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace petgo.api.Migrations
{
    /// <inheritdoc />
    public partial class CriarEstruturaDeDadosCompleta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fotos",
                table: "Pets");

            migrationBuilder.AlterColumn<string>(
                name: "FotoPerfil",
                table: "Usuarios",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Porte",
                table: "Pets",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "Pets",
                type: "TEXT",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Especie",
                table: "Pets",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "FotosJson",
                table: "Pets",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCriacao",
                table: "Avaliacoes",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "datetime('now')",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "AlvoTipo",
                table: "Avaliacoes",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "AnuncioDoacoes",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Moderacao",
                table: "AnuncioDoacoes",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "ContatoWhatsapp",
                table: "AnuncioDoacoes",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacao_Alvo",
                table: "Avaliacoes",
                columns: new[] { "AlvoTipo", "AlvoId" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Avaliacao_Nota",
                table: "Avaliacoes",
                sql: "[Nota] >= 1 AND [Nota] <= 5");

            migrationBuilder.CreateIndex(
                name: "IX_AnuncioDoacoes_Moderacao",
                table: "AnuncioDoacoes",
                column: "Moderacao");

            migrationBuilder.CreateIndex(
                name: "IX_AnuncioDoacoes_Status",
                table: "AnuncioDoacoes",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Avaliacao_Alvo",
                table: "Avaliacoes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Avaliacao_Nota",
                table: "Avaliacoes");

            migrationBuilder.DropIndex(
                name: "IX_AnuncioDoacoes_Moderacao",
                table: "AnuncioDoacoes");

            migrationBuilder.DropIndex(
                name: "IX_AnuncioDoacoes_Status",
                table: "AnuncioDoacoes");

            migrationBuilder.DropColumn(
                name: "FotosJson",
                table: "Pets");

            migrationBuilder.AlterColumn<string>(
                name: "FotoPerfil",
                table: "Usuarios",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Porte",
                table: "Pets",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "Pets",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Especie",
                table: "Pets",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Fotos",
                table: "Pets",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCriacao",
                table: "Avaliacoes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "datetime('now')");

            migrationBuilder.AlterColumn<int>(
                name: "AlvoTipo",
                table: "Avaliacoes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "AnuncioDoacoes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "Moderacao",
                table: "AnuncioDoacoes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ContatoWhatsapp",
                table: "AnuncioDoacoes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20);
        }
    }
}
