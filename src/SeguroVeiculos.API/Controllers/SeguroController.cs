using Microsoft.AspNetCore.Mvc;
using SeguroVeiculos.Application.UseCases.CriarSeguro;
using SeguroVeiculos.Application.UseCases.PesquisarSeguro;
using SeguroVeiculos.Application.UseCases.RelatorioMedias;

namespace SeguroVeiculos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeguroController : ControllerBase
{
    private readonly CriarSeguroHandler _criarHandler;
    private readonly PesquisarSeguroHandler _pesquisarHandler;
    private readonly RelatorioMediasHandler _relatorioHandler;

    public SeguroController(
        CriarSeguroHandler criarHandler,
        PesquisarSeguroHandler pesquisarHandler,
        RelatorioMediasHandler relatorioHandler)
    {
        _criarHandler = criarHandler;
        _pesquisarHandler = pesquisarHandler;
        _relatorioHandler = relatorioHandler;
    }

    /// <summary>Registra um novo seguro de veículo.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CriarSeguroResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] CriarSeguroCommand command, CancellationToken ct)
    {
        try
        {
            var resultado = await _criarHandler.ExecutarAsync(command, ct);
            return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    /// <summary>Pesquisa um seguro pelo ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PesquisarSeguroResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken ct)
    {
        var resultado = await _pesquisarHandler.ExecutarAsync(id, ct);
        return resultado is null ? NotFound() : Ok(resultado);
    }

    /// <summary>Retorna o relatório com as médias aritméticas dos seguros.</summary>
    [HttpGet("relatorio/medias")]
    [ProducesResponseType(typeof(RelatorioMediasResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RelatorioMedias(CancellationToken ct)
    {
        var resultado = await _relatorioHandler.ExecutarAsync(ct);
        return Ok(resultado);
    }
}

