using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servico.Estoque.Application.DTOs
{
    public class ProdutoDTO
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public int Saldo { get; set; }
        public bool Ativo { get; set; }
    }
}