using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Servico.Estoque.Domain.Entities;

namespace Servico.Estoque.Domain.Repositories
{
    public interface IProdutoRepository
    {
        Task<Produto?> ObterPorCodigoAsync(int Codigo);
        
        Task<Produto?> ObterPorCodigoComTrackingAsync(int codigo);

        Task<IEnumerable<Produto>> ObterTodosAsync();

        Task AdicionarAsync(Produto produto);

        Task AtualizarAsync(Produto produto);
       
    }
}