using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowFeedback.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarInformacoesTenantUnidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CorPrimariaOverride",
                table: "Unidades",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorSecundariaOverride",
                table: "Unidades",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrlOverride",
                table: "Unidades",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorPrimaria",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorSecundaria",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorPrimariaOverride",
                table: "Unidades");

            migrationBuilder.DropColumn(
                name: "CorSecundariaOverride",
                table: "Unidades");

            migrationBuilder.DropColumn(
                name: "LogoUrlOverride",
                table: "Unidades");

            migrationBuilder.DropColumn(
                name: "CorPrimaria",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "CorSecundaria",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Tenants");
        }
    }
}
