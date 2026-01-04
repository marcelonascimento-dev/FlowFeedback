using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowFeedback.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarEstruturaHierarquica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Dispositivos_TenantId_UnidadeId",
                table: "Dispositivos");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Unidades",
                newName: "Endereco");

            migrationBuilder.RenameColumn(
                name: "CodigoExterno",
                table: "Unidades",
                newName: "Cidade");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "AlvosAvaliacao",
                newName: "UnidadeId");

            migrationBuilder.AddColumn<string>(
                name: "NomeLoja",
                table: "Unidades",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "AlvosAvaliacao",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Subtitulo",
                table: "AlvosAvaliacao",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImagemUrl",
                table: "AlvosAvaliacao",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeCorporativo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Cnpj = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Votos_AlvoAvaliacaoId",
                table: "Votos",
                column: "AlvoAvaliacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivos_UnidadeId",
                table: "Dispositivos",
                column: "UnidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_AlvosAvaliacao_UnidadeId",
                table: "AlvosAvaliacao",
                column: "UnidadeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Votos_AlvoAvaliacaoId",
                table: "Votos");

            migrationBuilder.DropIndex(
                name: "IX_Dispositivos_UnidadeId",
                table: "Dispositivos");

            migrationBuilder.DropIndex(
                name: "IX_AlvosAvaliacao_UnidadeId",
                table: "AlvosAvaliacao");

            migrationBuilder.DropColumn(
                name: "NomeLoja",
                table: "Unidades");

            migrationBuilder.RenameColumn(
                name: "Endereco",
                table: "Unidades",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "Cidade",
                table: "Unidades",
                newName: "CodigoExterno");

            migrationBuilder.RenameColumn(
                name: "UnidadeId",
                table: "AlvosAvaliacao",
                newName: "TenantId");

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "AlvosAvaliacao",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Subtitulo",
                table: "AlvosAvaliacao",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ImagemUrl",
                table: "AlvosAvaliacao",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivos_TenantId_UnidadeId",
                table: "Dispositivos",
                columns: new[] { "TenantId", "UnidadeId" });
        }
    }
}
