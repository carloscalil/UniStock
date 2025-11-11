using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Servico.Faturamento.Domain.Entities;

namespace Servico.Faturamento.Application.DTOs
{
    public class NotaFiscalDTO
    {
        public int Numero { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<ItemNotaFiscalDTO> Itens { get; set; } = new List<ItemNotaFiscalDTO>();

        public static NotaFiscalDTO DeEntidade(NotaFiscal nota)
        {
            return new NotaFiscalDTO
            {
                Numero = nota.Numero,
                Status = nota.Status.ToString(),
                Itens = nota.Itens.Select(item => new ItemNotaFiscalDTO
                {
                    ProdutoCodigo = item.ProdutoCodigo,
                    Quantidade = item.Quantidade
                }).ToList()
            };
        }
    }
}