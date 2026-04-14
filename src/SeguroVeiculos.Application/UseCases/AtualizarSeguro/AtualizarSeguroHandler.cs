using SeguroVeiculos.Application.UseCases.PesquisarSeguro;
using SeguroVeiculos.Domain.Common;
using SeguroVeiculos.Domain.Interfaces;

namespace SeguroVeiculos.Application.UseCases.AtualizarSeguro;

public class AtualizarSeguroHandler
{
    private readonly ISeguroRepository _repository;

    public AtualizarSeguroHandler(ISeguroRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PesquisarSeguroResponse>> ExecutarAsync(Guid id, AtualizarSeguroCommand command,
                                                                  CancellationToken cancellationToken = default)
    {
        var seguro = await _repository.ObterPorIdAsync(id, cancellationToken);

        if (seguro is null)
            return Result<PesquisarSeguroResponse>.NotFound($"Seguro com id '{id}' não encontrado.");

        var resultado = seguro.Atualizar(
            command.ValorVeiculo,
            command.MarcaModeloVeiculo,
            command.NomeSegurado,
            command.CpfSegurado,
            command.IdadeSegurado);

        if (!resultado.IsSuccess)
            return Result<PesquisarSeguroResponse>.Fail(resultado.Error!, resultado.ErrorType);

        await _repository.AtualizarAsync(seguro, cancellationToken);

        var response = new PesquisarSeguroResponse(
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

        return Result<PesquisarSeguroResponse>.Ok(response);
    }
}
