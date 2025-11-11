using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Servico.Faturamento.Application.DTOs
{
    public class CriarNotaFiscalDTO
    {
        [Required]
        [MinLength(1, ErrorMessage = "A nota fiscal deve ter ao menos um item.")]
        public List<ItemNotaDTO> Itens { get; set; } = new List<ItemNotaDTO>();
    }
}