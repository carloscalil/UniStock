using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servico.Estoque.Application.DTOs
{
    public class CriarProdutoDTO
    {
        public required string Descricao { get; set; }
        public required int Saldo { get; set; }
    }
}