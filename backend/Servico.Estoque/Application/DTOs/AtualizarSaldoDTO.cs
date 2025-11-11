using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servico.Estoque.Application.DTOs
{
    public class AtualizarSaldoDTO
    {
        public required int Quantidade { get; set; }
    }
}