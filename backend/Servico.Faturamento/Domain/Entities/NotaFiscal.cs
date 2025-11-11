using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Servico.Faturamento.Domain.Enums;

namespace Servico.Faturamento.Domain.Entities
{
    public class NotaFiscal
    {
        public int Numero { get; set; }
        public ENotaFiscalStatus Status { get; set; }
        public DateTime DataEmissao { get; set; }
        public List<ItemNotaFiscal> Itens { get; set; } = new List<ItemNotaFiscal>();

        public NotaFiscal()
        {
            Status = ENotaFiscalStatus.Aberta;
            DataEmissao = DateTime.UtcNow;
        }

        public void Fechar()
        {
            if (Status != ENotaFiscalStatus.Aberta)
            {
                throw new InvalidOperationException("Apenas notas com status 'Aberta' podem ser fechadas.");
            }
            Status = ENotaFiscalStatus.Fechada;
        }

        public void AdicionarItem(int produtoCodigo, int quantidade)
        {
            var novoItem = new ItemNotaFiscal(produtoCodigo, quantidade);
            
            Itens.Add(novoItem);
        }
    }
}