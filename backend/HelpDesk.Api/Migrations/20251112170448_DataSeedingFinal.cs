using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HelpDesk.Api.Migrations
{
    /// <inheritdoc />
    public partial class DataSeedingFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "id_categoria", "nome_categoria" },
                values: new object[,]
                {
                    { 1, "Hardware" },
                    { 2, "Software" },
                    { 3, "Acesso" },
                    { 4, "Rede" },
                    { 5, "Sistema" }
                });

            migrationBuilder.InsertData(
                table: "Setores",
                columns: new[] { "id_setor", "nome_setor" },
                values: new object[,]
                {
                    { 1, "Financeiro" },
                    { 2, "TI" },
                    { 3, "RH" },
                    { 4, "Administrativo" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "DataCriacao", "Email", "Nome", "Perfil", "SenhaHash", "SetorIdSetor" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 12, 22, 10, 0, 0, 0, DateTimeKind.Utc), "kaue@empresa.com", "Kaue", "Analista", "$2b$10$lZ7kIf6XkTZIXCm6aEBh7eiplH0IZ8IqfTXKpp.sRQdihHK9A5FYO", 1 },
                    { 2, new DateTime(2024, 12, 23, 10, 0, 0, 0, DateTimeKind.Utc), "julia.castro@empresa.com", "Julia Castro", "Analista", "$2b$10$lZ7kIf6XkTZIXCm6aEBh7eiplH0IZ8IqfTXKpp.sRQdihHK9A5FYO", 1 },
                    { 3, new DateTime(2024, 12, 24, 10, 0, 0, 0, DateTimeKind.Utc), "vinicius.wayna@empresa.com", "Vinicius Wayna", "Usuario", "$2b$10$lZ7kIf6XkTZIXCm6aEBh7eiplH0IZ8IqfTXKpp.sRQdihHK9A5FYO", 3 },
                    { 6, new DateTime(2024, 12, 25, 10, 0, 0, 0, DateTimeKind.Utc), "vinicius.macedo@empresa.com", "Vinicius Macedo", "Admin", "$2b$10$lZ7kIf6XkTZIXCm6aEBh7eiplH0IZ8IqfTXKpp.sRQdihHK9A5FYO", 4 },
                    { 7, new DateTime(2024, 12, 26, 10, 0, 0, 0, DateTimeKind.Utc), "irineu.julio@empresa.com", "Irineu Julio", "Usuario", "$2b$10$lZ7kIf6XkTZIXCm6aEBh7eiplH0IZ8IqfTXKpp.sRQdihHK9A5FYO", 2 },
                    { 8, new DateTime(2024, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "samuel.nobre@empresa.com", "Samuel NObRe", "Usuario", "$2b$10$lZ7kIf6XkTZIXCm6aEBh7eiplH0IZ8IqfTXKpp.sRQdihHK9A5FYO", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "id_categoria",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "id_categoria",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "id_categoria",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "id_categoria",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "id_categoria",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Setores",
                keyColumn: "id_setor",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Setores",
                keyColumn: "id_setor",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Setores",
                keyColumn: "id_setor",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Setores",
                keyColumn: "id_setor",
                keyValue: 4);
        }
    }
}
