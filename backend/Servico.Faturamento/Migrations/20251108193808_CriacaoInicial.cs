using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Servico.Faturamento.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotasFiscais",
                columns: table => new
                {
                    Numero = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DataEmissao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasFiscais", x => x.Numero);
                });

            migrationBuilder.CreateTable(
                name: "ItensNotaFiscal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdutoCodigo = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    NotaFiscalNumero = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensNotaFiscal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensNotaFiscal_NotasFiscais_NotaFiscalNumero",
                        column: x => x.NotaFiscalNumero,
                        principalTable: "NotasFiscais",
                        principalColumn: "Numero",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItensNotaFiscal_NotaFiscalNumero",
                table: "ItensNotaFiscal",
                column: "NotaFiscalNumero");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItensNotaFiscal");

            migrationBuilder.DropTable(
                name: "NotasFiscais");
        }
    }
}
