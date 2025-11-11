using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Servico.Estoque.Application.DTOs
{
    public class AtualizarProdutoDTO
    {
        [Required(ErrorMessage = "Descrição é obrigatória.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Saldo é obrigatório.")]
        [Range(0, int.MaxValue, ErrorMessage = "Saldo não pode ser negativo.")]
        public int Saldo { get; set; }
    }
}