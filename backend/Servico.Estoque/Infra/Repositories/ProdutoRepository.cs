using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Servico.Estoque.Domain.Entities;
using Servico.Estoque.Domain.Repositories;
using Servico.Estoque.Infra.Data;

namespace Servico.Estoque.Infra.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AdicionarAsync(Produto produto)
        {
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task<Produto?> ObterPorCodigoAsync(int Codigo)
        {
            return await _context.Produtos
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(p => p.Codigo == Codigo);
        }

        public async Task<Produto?> ObterPorCodigoComTrackingAsync(int codigo)
        {
            return await _context.Produtos
                                 .FirstOrDefaultAsync(p => p.Codigo == codigo);
        }

        public async Task<IEnumerable<Produto>> ObterTodosAsync()
        {
            return await _context.Produtos
                                 .AsNoTracking()
                                 .Where(p => p.Ativo == true) 
                                 .ToListAsync();
        }
    }
}