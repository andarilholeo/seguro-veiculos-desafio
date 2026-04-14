using Microsoft.AspNetCore.Mvc;
using SeguroVeiculos.Application.UseCases.AtualizarSeguro;
using SeguroVeiculos.Application.UseCases.CriarSeguro;
using SeguroVeiculos.Application.UseCases.ListarSeguros;
using SeguroVeiculos.Application.UseCases.PesquisarSeguro;
using SeguroVeiculos.Application.UseCases.RelatorioMedias;
using SeguroVeiculos.Application.UseCases.RemoverSeguro;
using SeguroVeiculos.Domain.Common;

namespace SeguroVeiculos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeguroController : ControllerBase
{
    private readonly CriarSeguroHandler _criarHandler;
    private readonly PesquisarSeguroHandler _pesquisarHandler;
    private readonly ListarSegurosHandler _listarHandler;
    private readonly AtualizarSeguroHandler _atualizarHandler;
    private readonly RemoverSeguroHandler _removerHandler;
    private readonly RelatorioMediasHandler _relatorioHandler;

    public SeguroController(
        CriarSeguroHandler criarHandler,
        PesquisarSeguroHandler pesquisarHandler,
        ListarSegurosHandler listarHandler,
        AtualizarSeguroHandler atualizarHandler,
        RemoverSeguroHandler removerHandler,
        RelatorioMediasHandler relatorioHandler)
    {
        _criarHandler = criarHandler;
        _pesquisarHandler = pesquisarHandler;
        _listarHandler = listarHandler;
        _atualizarHandler = atualizarHandler;
        _removerHandler = removerHandler;
        _relatorioHandler = relatorioHandler;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CriarSeguroResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] CriarSeguroCommand command, CancellationToken ct)
    {
        var resultado = await _criarHandler.ExecutarAsync(command, ct);
        if (!resultado.IsSuccess)
            return ToErrorResponse(resultado.ErrorType, resultado.Error!);

        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Value!.Id }, resultado.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PesquisarSeguroResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken ct)
    {
        var resultado = await _pesquisarHandler.ExecutarAsync(id, ct);
        if (!resultado.IsSuccess)
            return ToErrorResponse(resultado.ErrorType, resultado.Error!);

        return Ok(resultado.Value);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PesquisarSeguroResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken ct)
    {
        var resultado = await _listarHandler.ExecutarAsync(ct);
        if (!resultado.IsSuccess)
            return ToErrorResponse(resultado.ErrorType, resultado.Error!);

        return Ok(resultado.Value);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PesquisarSeguroResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarSeguroCommand command, CancellationToken ct)
    {
        var resultado = await _atualizarHandler.ExecutarAsync(id, command, ct);
        if (!resultado.IsSuccess)
            return ToErrorResponse(resultado.ErrorType, resultado.Error!);

        return Ok(resultado.Value);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(Guid id, CancellationToken ct)
    {
        var resultado = await _removerHandler.ExecutarAsync(id, ct);
        if (!resultado.IsSuccess)
            return ToErrorResponse(resultado.ErrorType, resultado.Error!);

        return NoContent();
    }

    [HttpGet("relatorio/medias")]
    [ProducesResponseType(typeof(RelatorioMediasResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RelatorioMedias(CancellationToken ct)
    {
        var resultado = await _relatorioHandler.ExecutarAsync(ct);
        if (!resultado.IsSuccess)
            return ToErrorResponse(resultado.ErrorType, resultado.Error!);

        return Ok(resultado.Value);
    }

    private IActionResult ToErrorResponse(ResultErrorType errorType, string error) => errorType switch
    {
        ResultErrorType.NotFound   => NotFound(new { erro = error }),
        ResultErrorType.Validation => BadRequest(new { erro = error }),
        _                          => BadRequest(new { erro = error })
    };
}

