using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Servico.Estoque.Migrations
{
    /// <inheritdoc />
    public partial class RenomearStatusParaAtivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Produtos",
                newName: "Ativo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ativo",
                table: "Produtos",
                newName: "Status");
        }
    }
}
