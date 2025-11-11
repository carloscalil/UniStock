using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Servico.Faturamento.Application.DTOs
{
    public class ItemNotaDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Código do produto inválido.")]
        public int ProdutoCodigo { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser ao menos 1.")]
        public int Quantidade { get; set; }
    }
}