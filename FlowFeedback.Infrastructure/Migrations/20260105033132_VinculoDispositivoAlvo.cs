using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowFeedback.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VinculoDispositivoAlvo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Dispositivos_UnidadeId",
                table: "Dispositivos");

            migrationBuilder.CreateTable(
                name: "DispositivoAlvos",
                columns: table => new
                {
                    AlvosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DispositivosId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispositivoAlvos", x => new { x.AlvosId, x.DispositivosId });
                    table.ForeignKey(
                        name: "FK_DispositivoAlvos_AlvosAvaliacao_AlvosId",
                        column: x => x.AlvosId,
                        principalTable: "AlvosAvaliacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DispositivoAlvos_Dispositivos_DispositivosId",
                        column: x => x.DispositivosId,
                        principalTable: "Dispositivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DispositivoAlvos_DispositivosId",
                table: "DispositivoAlvos",
                column: "DispositivosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DispositivoAlvos");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivos_UnidadeId",
                table: "Dispositivos",
                column: "UnidadeId");
        }
    }
}
