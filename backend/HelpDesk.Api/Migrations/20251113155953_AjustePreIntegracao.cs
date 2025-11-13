using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjustePreIntegracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "SenhaHash",
                value: "$2b$10$BtrBZpxLvcisdmK295Cri.G3by.duKeWX/Og6uEGxQmuBxs9l6aWG");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "SenhaHash",
                value: "$2b$10$BtrBZpxLvcisdmK295Cri.G3by.duKeWX/Og6uEGxQmuBxs9l6aWG");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "SenhaHash",
                value: "$2b$10$BtrBZpxLvcisdmK295Cri.G3by.duKeWX/Og6uEGxQmuBxs9l6aWG");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 6,
                column: "SenhaHash",
                value: "$2b$10$BtrBZpxLvcisdmK295Cri.G3by.duKeWX/Og6uEGxQmuBxs9l6aWG");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 7,
                column: "SenhaHash",
                value: "$2b$10$BtrBZpxLvcisdmK295Cri.G3by.duKeWX/Og6uEGxQmuBxs9l6aWG");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 8,
                column: "SenhaHash",
                value: "$2b$10$BtrBZpxLvcisdmK295Cri.G3by.duKeWX/Og6uEGxQmuBxs9l6aWG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
