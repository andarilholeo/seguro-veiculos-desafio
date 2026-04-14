using SeguroVeiculos.Application.Interfaces;
using SeguroVeiculos.Domain.Common;

namespace SeguroVeiculos.Application.UseCases.ConsultarSegurado;

public class ConsultarSeguradoPorCpfHandler
{
    private readonly ISeguradoExternoService _seguradoExternoService;

    public ConsultarSeguradoPorCpfHandler(ISeguradoExternoService seguradoExternoService)
    {
        _seguradoExternoService = seguradoExternoService;
    }

    public async Task<Result<SeguradoExternoDto>> ExecutarAsync(string cpf, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return Result<SeguradoExternoDto>.Validation("O CPF é obrigatório.");

        var segurado = await _seguradoExternoService.ObterPorCpfAsync(cpf, cancellationToken);

        if (segurado is null)
            return Result<SeguradoExternoDto>.NotFound($"Nenhum segurado encontrado para o CPF '{cpf}'.");

        return Result<SeguradoExternoDto>.Ok(segurado);
    }
}
