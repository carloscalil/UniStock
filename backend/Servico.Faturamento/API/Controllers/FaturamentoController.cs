using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Servico.Faturamento.Application.DTOs;
using Servico.Faturamento.Application.Services;

namespace Servico.Faturamento.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaturamentoController : ControllerBase
    {
        private readonly FaturamentoService _faturamentoService;

        public FaturamentoController(FaturamentoService faturamentoService)
        {
            _faturamentoService = faturamentoService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(NotaFiscalDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CriarNotaFiscal([FromBody] CriarNotaFiscalDTO dto)
        {
            try
            {
                var novaNota = await _faturamentoService.CriarNotaFiscalAsync(dto);

                return CreatedAtAction(nameof(ObterNotaPorNumero), new { numeroNota = novaNota.Numero }, novaNota);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro interno: {ex.Message}");
            }
        }

        [HttpPost("{numeroNota}/imprimir")]
        [ProducesResponseType(typeof(NotaFiscalDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ImprimirNotaFiscal(int numeroNota)
        {
            try
            {
                var notaProcessada = await _faturamentoService.ProcessarImpressaoAsync(numeroNota);
                return Ok(notaProcessada);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NotaFiscalDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ObterTodas()
        {
            try
            {
                var notasDto = await _faturamentoService.ObterTodasAsync();
                return Ok(notasDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro interno: {ex.Message}");
            }
        }

        [HttpGet("{numeroNota}")] 
        [ProducesResponseType(typeof(NotaFiscalDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ObterNotaPorNumero(int numeroNota)
        {
            try
            {
                var nota = await _faturamentoService.ObterNotaPorNumeroAsync(numeroNota);
                if (nota == null)
                {
                    return NotFound($"Nota fiscal com número {numeroNota} não encontrada.");
                }
                return Ok(nota);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro interno: {ex.Message}");
            }
        }
    }
}