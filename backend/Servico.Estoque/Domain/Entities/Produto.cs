using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Servico.Estoque.Domain.Entities
{
    public class Produto
    {
        public int Codigo { get; set; }
        public required string Descricao { get; set; }
        public required int Saldo { get; set; }
        public bool Ativo { get; private set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }

        public Produto()
        {
            Ativo = true;
        }

        public void DebitarDoSaldo(int quantidadeADebitar)
        {
            if (quantidadeADebitar <= 0)
            {
                throw new ArgumentException("Quantidade a debitar deve ser positiva.");
            }
            if (Saldo - quantidadeADebitar < 0)
            {
                throw new InvalidOperationException($"Saldo insuficiente em estoque para o produto: '{Descricao}'.");
            }
            Saldo -= quantidadeADebitar;
        }
        
        public void Inativar()
        {
            Ativo = false;
        }

        public void Reativar()
        {
            Ativo = true;
        }

        public void AtualizarDados(string novaDescricao, int novoSaldo)
        {
            if (string.IsNullOrWhiteSpace(novaDescricao))
            {
                throw new InvalidOperationException("Descrição não pode ser vazia.");
            }
            if (novoSaldo < 0)
            {
                throw new InvalidOperationException("Saldo não pode ser negativo.");
            }
            
            Descricao = novaDescricao;
            Saldo = novoSaldo;
        }
    }
}