
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.Api.Migrations
{
    /// <inheritdoc />
    public partial class FinalModelV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Usuarios_TecnicoId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Usuarios_UsuarioId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios");

            migrationBuilder.AddColumn<int>(
                name: "SetorIdSetor",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    id_categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome_categoria = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.id_categoria);
                });

            migrationBuilder.CreateTable(
                name: "LogsAuditoria",
                columns: table => new
                {
                    id_log = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario_afetado = table.Column<int>(type: "int", nullable: false),
                    id_usuario_executor = table.Column<int>(type: "int", nullable: false),
                    acao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    tabela_afetada = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    data_hora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ip_origem = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsAuditoria", x => x.id_log);
                    table.ForeignKey(
                        name: "FK_LogsAuditoria_Usuarios_id_usuario_afetado",
                        column: x => x.id_usuario_afetado,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogsAuditoria_Usuarios_id_usuario_executor",
                        column: x => x.id_usuario_executor,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Setores",
                columns: table => new
                {
                    id_setor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome_setor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setores", x => x.id_setor);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_SetorIdSetor",
                table: "Usuarios",
                column: "SetorIdSetor");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_id_usuario_afetado",
                table: "LogsAuditoria",
                column: "id_usuario_afetado");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_id_usuario_executor",
                table: "LogsAuditoria",
                column: "id_usuario_executor");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Usuarios_TecnicoId",
                table: "Tickets",
                column: "TecnicoId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Usuarios_UsuarioId",
                table: "Tickets",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Setores_SetorIdSetor",
                table: "Usuarios",
                column: "SetorIdSetor",
                principalTable: "Setores",
                principalColumn: "id_setor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Usuarios_TecnicoId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Usuarios_UsuarioId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Setores_SetorIdSetor",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "LogsAuditoria");

            migrationBuilder.DropTable(
                name: "Setores");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_SetorIdSetor",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "SetorIdSetor",
                table: "Usuarios");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Usuarios_TecnicoId",
                table: "Tickets",
                column: "TecnicoId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Usuarios_UsuarioId",
                table: "Tickets",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
