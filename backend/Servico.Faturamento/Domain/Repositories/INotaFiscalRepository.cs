using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Servico.Faturamento.Domain.Entities;

namespace Servico.Faturamento.Domain.Repositories
{
    public interface INotaFiscalRepository
    {
        Task<NotaFiscal?> ObterPorNumeroAsync(int numero);
        Task<IEnumerable<NotaFiscal>> ObterTodasAsync();

        Task AdicionarAsync(NotaFiscal notaFiscal);

        Task AtualizarAsync(NotaFiscal notaFiscal);
    }
}