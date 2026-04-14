using SeguroVeiculos.Application.Interfaces;
using SeguroVeiculos.Domain.Common;
using SeguroVeiculos.Domain.Entities;
using SeguroVeiculos.Domain.Interfaces;

namespace SeguroVeiculos.Application.UseCases.CriarSeguro;

public class CriarSeguroHandler
{
    private readonly ISeguroRepository _repository;
    private readonly ISeguradoExternoService _seguradoExternoService;

    public CriarSeguroHandler(ISeguroRepository repository, ISeguradoExternoService seguradoExternoService)
    {
        _repository = repository;
        _seguradoExternoService = seguradoExternoService;
    }

    public async Task<Result<CriarSeguroResponse>> ExecutarAsync(CriarSeguroCommand command, CancellationToken cancellationToken = default)
    {
        var seguradoExterno = await _seguradoExternoService.ObterPorCpfAsync(command.CpfSegurado, cancellationToken);

        var nome = seguradoExterno?.Nome ?? command.NomeSegurado;
        var idade = seguradoExterno?.Idade ?? command.IdadeSegurado;

        var resultadoSegurado = Segurado.Criar(nome, command.CpfSegurado, idade);
        if (!resultadoSegurado.IsSuccess)
            return Result<CriarSeguroResponse>.Validation(resultadoSegurado.Error!);

        try
        {
            var veiculo = new Veiculo(command.ValorVeiculo, command.MarcaModeloVeiculo);
            var seguro = new Seguro(veiculo, resultadoSegurado.Value!);

            await _repository.AdicionarAsync(seguro, cancellationToken);

            var response = new CriarSeguroResponse(
                seguro.Id,
                seguro.Veiculo.MarcaModelo,
                seguro.Veiculo.Valor,
                seguro.Segurado.Nome,
                seguro.Segurado.Cpf,
                seguro.Segurado.Idade,
                seguro.Calculo.TaxaRisco,
                seguro.Calculo.PremioRisco,
                seguro.Calculo.PremioPuro,
                seguro.Calculo.PremioComercial,
                seguro.CriadoEm
            );

            return Result<CriarSeguroResponse>.Ok(response);
        }
        catch (ArgumentException ex)
        {
            return Result<CriarSeguroResponse>.Validation(ex.Message);
        }
    }
}

