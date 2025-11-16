using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.Api.Migrations
{
    /// <inheritdoc />
    public partial class FinalBackendSecurityFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Setores_SetorIdSetor",
                table: "Usuarios");

            migrationBuilder.AlterColumn<int>(
                name: "SetorIdSetor",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "tabela_afetada",
                table: "LogsAuditoria",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ip_origem",
                table: "LogsAuditoria",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Setores_SetorIdSetor",
                table: "Usuarios",
                column: "SetorIdSetor",
                principalTable: "Setores",
                principalColumn: "id_setor",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Setores_SetorIdSetor",
                table: "Usuarios");

            migrationBuilder.AlterColumn<int>(
                name: "SetorIdSetor",
                table: "Usuarios",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "tabela_afetada",
                table: "LogsAuditoria",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ip_origem",
                table: "LogsAuditoria",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Setores_SetorIdSetor",
                table: "Usuarios",
                column: "SetorIdSetor",
                principalTable: "Setores",
                principalColumn: "id_setor");
        }
    }
}
