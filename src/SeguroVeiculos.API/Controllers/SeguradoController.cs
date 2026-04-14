using Microsoft.AspNetCore.Mvc;
using SeguroVeiculos.Application.Interfaces;
using SeguroVeiculos.Application.UseCases.ConsultarSegurado;
using SeguroVeiculos.Domain.Common;

namespace SeguroVeiculos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeguradoController : ControllerBase
{
    private readonly ConsultarSeguradoPorCpfHandler _porCpfHandler;
    private readonly ConsultarSeguradoPorNomeHandler _porNomeHandler;

    public SeguradoController(
        ConsultarSeguradoPorCpfHandler porCpfHandler,
        ConsultarSeguradoPorNomeHandler porNomeHandler)
    {
        _porCpfHandler = porCpfHandler;
        _porNomeHandler = porNomeHandler;
    }

    [HttpGet("{cpf}")]
    [ProducesResponseType(typeof(SeguradoExternoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorCpf(string cpf, CancellationToken ct)
    {
        var resultado = await _porCpfHandler.ExecutarAsync(cpf, ct);
        if (!resultado.IsSuccess)
            return ToErrorResponse(resultado.ErrorType, resultado.Error!);

        return Ok(resultado.Value);
    }

    [HttpGet("nome/{nome}")]
    [ProducesResponseType(typeof(IEnumerable<SeguradoExternoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorNome(string nome, CancellationToken ct)
    {
        var resultado = await _porNomeHandler.ExecutarAsync(nome, ct);
        if (!resultado.IsSuccess)
            return ToErrorResponse(resultado.ErrorType, resultado.Error!);

        return Ok(resultado.Value);
    }

    private IActionResult ToErrorResponse(ResultErrorType errorType, string error) => errorType switch
    {
        ResultErrorType.NotFound   => NotFound(new { error }),
        ResultErrorType.Validation => BadRequest(new { error }),
        _                          => StatusCode(500, new { error })
    };
}
