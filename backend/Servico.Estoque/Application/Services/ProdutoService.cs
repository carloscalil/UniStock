using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Servico.Estoque.Application.DTOs;
using Servico.Estoque.Domain.Entities;
using Servico.Estoque.Domain.Repositories;

namespace Servico.Estoque.Application.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<ProdutoDTO> CriarProdutoAsync(CriarProdutoDTO dto)
        {
            var produto = new Produto
            {
                Descricao = dto.Descricao,
                Saldo = dto.Saldo
            };

            await _produtoRepository.AdicionarAsync(produto);

            return new ProdutoDTO
            {
                Codigo = produto.Codigo,
                Descricao = produto.Descricao,
                Saldo = produto.Saldo,
                Ativo = produto.Ativo
            };
        }

        public async Task<IEnumerable<ProdutoDTO>> ObterTodosAsync()
        {
            var produtos = await _produtoRepository.ObterTodosAsync();
            return produtos.Select(p => new ProdutoDTO
            {
                Codigo = p.Codigo,
                Descricao = p.Descricao,
                Saldo = p.Saldo,
                Ativo = p.Ativo
            });
        }

        public async Task<ProdutoDTO?> ObterPorCodigoAsync(int codigo)
        {
            var produto = await _produtoRepository.ObterPorCodigoAsync(codigo);
            if (produto == null) return null;

            return new ProdutoDTO
            {
                Codigo = produto.Codigo,
                Descricao = produto.Descricao,
                Saldo = produto.Saldo,
                Ativo = produto.Ativo
            };
        }

        public async Task<ProdutoDTO> AtualizarProdutoAsync(int codigo, AtualizarProdutoDTO dto)
        {
            // Usa o método COM TRACKING
            var produto = await _produtoRepository.ObterPorCodigoComTrackingAsync(codigo);
            if (produto == null)
            {
                throw new KeyNotFoundException("Produto não encontrado.");
            }

            produto.AtualizarDados(dto.Descricao, dto.Saldo);

            await _produtoRepository.AtualizarAsync(produto);

            return new ProdutoDTO
            {
                Codigo = produto.Codigo,
                Descricao = produto.Descricao,
                Saldo = produto.Saldo,
                Ativo = produto.Ativo,
            };
        }

        public async Task AtualizarSaldoAsync(int codigo, AtualizarSaldoDTO dto)
        {
            var produto = await _produtoRepository.ObterPorCodigoAsync(codigo);

            if (produto == null)
            {
                throw new KeyNotFoundException("Produto não encontrado.");
            }

            produto.DebitarDoSaldo(dto.Quantidade);

            try
            {
                await _produtoRepository.AtualizarAsync(produto);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidOperationException("O saldo do produto foi alterado por outro usuário. Tente novamente.");
            }
        }
        
        public async Task InativarProdutoAsync(int codigo)
        {
            var produto = await _produtoRepository.ObterPorCodigoAsync(codigo);
            if (produto == null)
            {
                throw new KeyNotFoundException("Produto não encontrado.");
            }

            produto.Inativar();
            await _produtoRepository.AtualizarAsync(produto);
        }
    }
}