using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servico.Estoque.Application.DTOs;
using Servico.Estoque.Application.Services;

namespace Servico.Estoque.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutoService _produtoService;

        public ProdutosController(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProdutoDTO>), 200)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                var produtos = await _produtoService.ObterTodosAsync();
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocorreu um erro interno: {ex.Message}" });
            }
        }

        [HttpGet("{codigo}")]
        [ProducesResponseType(typeof(ProdutoDTO), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> ObterPorCodigo(int codigo)
        {
            try
            {
                var produto = await _produtoService.ObterPorCodigoAsync(codigo);
                if (produto == null)
                {
                    return NotFound(new { message = "Produto não encontrado." });
                }
                return Ok(produto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocorreu um erro interno: {ex.Message}" });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProdutoDTO), 201)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> CriarProduto([FromBody] CriarProdutoDTO dto)
        {
            try
            {
                var produtoDto = await _produtoService.CriarProdutoAsync(dto);
                return CreatedAtAction(nameof(ObterPorCodigo), new { codigo = produtoDto.Codigo }, produtoDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocorreu um erro interno: {ex.Message}" });
            }
        }

        [HttpPut("{codigo}")]
        [ProducesResponseType(typeof(ProdutoDTO), 200)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 409)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> AtualizarProduto(int codigo, [FromBody] AtualizarProdutoDTO dto)
        {
            try
            {
                var produtoDto = await _produtoService.AtualizarProdutoAsync(codigo, dto);
                return Ok(produtoDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "O produto foi modificado por outro usuário. Tente novamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocorreu um erro interno: {ex.Message}" });
            }
        }

        [HttpPut("{codigo}/atualizar-saldo")]
        [ProducesResponseType(200)] // Sucesso
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 409)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> AtualizarSaldo(int codigo, [FromBody] AtualizarSaldoDTO dto)
        {
            try
            {
                await _produtoService.AtualizarSaldoAsync(codigo, dto);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "O produto foi modificado por outro usuário. Tente novamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocorreu um erro interno: {ex.Message}" });
            }
        }
        [HttpDelete("{codigo}")]
        [ProducesResponseType(204)] 
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> InativarProduto(int codigo)
        {
            try
            {
                await _produtoService.InativarProdutoAsync(codigo);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocorreu um erro interno: {ex.Message}" });
            }
        }
    }
}