using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Servico.Faturamento.Application.DTOs;
using Servico.Faturamento.Domain.Entities;
using Servico.Faturamento.Domain.Enums;
using Servico.Faturamento.Domain.Repositories;

namespace Servico.Faturamento.Application.Services
{
    public class FaturamentoService
    {
        private readonly INotaFiscalRepository _repository;
        private readonly IHttpClientFactory _httpClientFactory;

        private const string _servicoEstoqueUrl = "http://localhost:5142";

        public FaturamentoService(INotaFiscalRepository repository, IHttpClientFactory httpClientFactory)
        {
            _repository = repository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<NotaFiscalDTO> CriarNotaFiscalAsync(CriarNotaFiscalDTO dto)
        {
            var notaFiscal = new NotaFiscal();

            foreach (var itemDto in dto.Itens)
            {
                notaFiscal.AdicionarItem(itemDto.ProdutoCodigo, itemDto.Quantidade);
            }

            await _repository.AdicionarAsync(notaFiscal);

            return NotaFiscalDTO.DeEntidade(notaFiscal);
        }

        public async Task<NotaFiscalDTO> ProcessarImpressaoAsync(int numeroNota)
        {
            var nota = await _repository.ObterPorNumeroAsync(numeroNota);

            if (nota == null)
            {
                throw new KeyNotFoundException("Nota Fiscal não encontrada.");
            }
            if (nota.Status != ENotaFiscalStatus.Aberta)
            {
                throw new InvalidOperationException("Esta nota fiscal não está aberta e não pode ser processada.");
            }
            if (!nota.Itens.Any())
            {
                throw new InvalidOperationException("Nota fiscal não possui itens.");
            }

            var httpClient = _httpClientFactory.CreateClient();

            try
            {
                foreach (var item in nota.Itens)
                {
                    string jsonBody = $"{{ \"Quantidade\": {item.Quantidade} }}"; 
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    var endpoint = $"/api/produtos/{item.ProdutoCodigo}/atualizar-saldo";

                    var response = await httpClient.PutAsync(_servicoEstoqueUrl + endpoint, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        var erro = await response.Content.ReadAsStringAsync();
                        string mensagemErro;
                        try
                        {                                           
                            var erroObj = JsonDocument.Parse(erro);
                            
                            mensagemErro = erroObj.RootElement.TryGetProperty("message", out var msg) 
                                           ? msg.GetString() ?? "Erro desconhecido no estoque."
                                           : "Erro desconhecido no estoque.";
                        }
                        catch
                        {
                            mensagemErro = $"Falha ao atualizar produto {item.ProdutoCodigo}.";
                        }
                        
                        throw new InvalidOperationException(mensagemErro);
                    }
                }
            }
            catch (InvalidOperationException ex) 
            {
                throw;
            }
            catch (HttpRequestException ex) 
            {
                throw new Exception($"Não foi possível conectar ao Serviço de Estoque. A nota NÃO foi fechada. Detalhe: {ex.Message}");
            }

            nota.Fechar();
            await _repository.AtualizarAsync(nota);
            return NotaFiscalDTO.DeEntidade(nota);
        }

        public async Task<NotaFiscalDTO?> ObterNotaPorNumeroAsync(int numero)
        {
            var nota = await _repository.ObterPorNumeroAsync(numero);

            if (nota == null)
            {
                return null;
            }

            return NotaFiscalDTO.DeEntidade(nota);
        }

        public async Task<IEnumerable<NotaFiscalDTO>> ObterTodasAsync()
        {
            var notas = await _repository.ObterTodasAsync();
            
            return notas.Select(nota => NotaFiscalDTO.DeEntidade(nota));
        }
    }
}