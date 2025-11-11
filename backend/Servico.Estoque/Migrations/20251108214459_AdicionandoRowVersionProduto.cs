using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Servico.Estoque.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoRowVersionProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Produtos",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Produtos");
        }
    }
}
