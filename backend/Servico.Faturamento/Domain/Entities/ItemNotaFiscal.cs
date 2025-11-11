using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servico.Faturamento.Domain.Entities
{
    public class ItemNotaFiscal
    {
        public int Id { get; set; }
        
        public int ProdutoCodigo { get; set; } 
        
        public int Quantidade { get; set; }

        public int NotaFiscalNumero { get; set; } 
        public NotaFiscal? NotaFiscal { get; set; }

        public ItemNotaFiscal() { }

        public ItemNotaFiscal(int produtoCodigo, int quantidade)
        {
            if (quantidade <= 0)
            {
                throw new ArgumentException("A quantidade deve ser maior que zero.");
            }
            ProdutoCodigo = produtoCodigo;
            Quantidade = quantidade;
        }
    }
}