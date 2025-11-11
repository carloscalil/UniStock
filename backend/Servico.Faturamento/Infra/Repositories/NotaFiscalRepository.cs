using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Servico.Faturamento.Domain.Entities;
using Servico.Faturamento.Domain.Repositories;
using Servico.Faturamento.Infra.Data;

namespace Servico.Faturamento.Infra.Repositories
{
    public class NotaFiscalRepository : INotaFiscalRepository
    {
        private readonly AppDbContext _context;
        public NotaFiscalRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AdicionarAsync(NotaFiscal notaFiscal)
        {
            await _context.NotasFiscais.AddAsync(notaFiscal);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(NotaFiscal notaFiscal)
        {
            _context.NotasFiscais.Update(notaFiscal);
            await _context.SaveChangesAsync();
        }

        public async Task<NotaFiscal?> ObterPorNumeroAsync(int numero)
        {
            return await _context.NotasFiscais
                                 .Include(n => n.Itens) 
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(n => n.Numero == numero);
        }

        public async Task<IEnumerable<NotaFiscal>> ObterTodasAsync()
        {
            return await _context.NotasFiscais
                                .Include(n => n.Itens)
                                .AsNoTracking()
                                .OrderByDescending(n => n.Numero)
                                .ToListAsync();
        }
    }
}