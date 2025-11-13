using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResumoTriagem",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SetorRecomendado",
                table: "Tickets",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SolucaoSugerida",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "SenhaHash",
                value: "$2b$10$CMnXwo5bW8fyV/RBX2kRL.wTWEqF6vuan5Iew3/SH6IeLtS0FWBae");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "SenhaHash",
                value: "$2b$10$CMnXwo5bW8fyV/RBX2kRL.wTWEqF6vuan5Iew3/SH6IeLtS0FWBae");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "SenhaHash",
                value: "$2b$10$CMnXwo5bW8fyV/RBX2kRL.wTWEqF6vuan5Iew3/SH6IeLtS0FWBae");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 6,
                column: "SenhaHash",
                value: "$2b$10$CMnXwo5bW8fyV/RBX2kRL.wTWEqF6vuan5Iew3/SH6IeLtS0FWBae");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 7,
                column: "SenhaHash",
                value: "$2b$10$CMnXwo5bW8fyV/RBX2kRL.wTWEqF6vuan5Iew3/SH6IeLtS0FWBae");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 8,
                column: "SenhaHash",
                value: "$2b$10$CMnXwo5bW8fyV/RBX2kRL.wTWEqF6vuan5Iew3/SH6IeLtS0FWBae");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResumoTriagem",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SetorRecomendado",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SolucaoSugerida",
                table: "Tickets");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "SenhaHash",
                value: "$2b$10$lZ7kIf6XkTZIXCm6aEBh7eiplH0IZ8IqfTXKpp.sRQdihHK9A5FYO");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "SenhaHash",
                value: "$2b$10$lZ7kIf6XkTZIXCm6aEBh7eiplH0IZ8IqfTXKpp.sRQdihHK9A5FYO");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "SenhaHash",
                value: "$2b$10$lZ7kIf6XkTZIXCm6aEBh7eiplH0IZ8IqfTXKpp.sRQdihHK9A5FYO");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 6,
                column: "SenhaHash",
                value: "$2b$10$lZ7kIf6XkTZIXCm6aEBh7eiplH0IZ8IqfTXKpp.sRQdihHK9A5FYO");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 7,
                column: "SenhaHash",
                value: "$2b$10$lZ7kIf6XkTZIXCm6aEBh7eiplH0IZ8IqfTXKpp.sRQdihHK9A5FYO");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 8,
                column: "SenhaHash",
                value: "$2b$10$lZ7kIf6XkTZIXCm6aEBh7eiplH0IZ8IqfTXKpp.sRQdihHK9A5FYO");
        }
    }
}
