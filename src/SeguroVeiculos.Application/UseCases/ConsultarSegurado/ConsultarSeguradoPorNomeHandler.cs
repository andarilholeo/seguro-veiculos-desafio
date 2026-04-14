using SeguroVeiculos.Application.Interfaces;
using SeguroVeiculos.Domain.Common;

namespace SeguroVeiculos.Application.UseCases.ConsultarSegurado;

public class ConsultarSeguradoPorNomeHandler
{
    private readonly ISeguradoExternoService _seguradoExternoService;

    public ConsultarSeguradoPorNomeHandler(ISeguradoExternoService seguradoExternoService)
    {
        _seguradoExternoService = seguradoExternoService;
    }

    public async Task<Result<IEnumerable<SeguradoExternoDto>>> ExecutarAsync(string nome, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result<IEnumerable<SeguradoExternoDto>>.Validation("O nome é obrigatório.");

        var segurados = await _seguradoExternoService.ObterPorNomeAsync(nome, cancellationToken);

        if (!segurados.Any())
            return Result<IEnumerable<SeguradoExternoDto>>.NotFound($"Nenhum segurado encontrado com o nome '{nome}'.");

        return Result<IEnumerable<SeguradoExternoDto>>.Ok(segurados);
    }
}
