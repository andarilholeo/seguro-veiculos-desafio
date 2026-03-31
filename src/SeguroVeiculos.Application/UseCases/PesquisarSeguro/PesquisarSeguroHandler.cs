using SeguroVeiculos.Domain.Interfaces;

namespace SeguroVeiculos.Application.UseCases.PesquisarSeguro;

public class PesquisarSeguroHandler
{
    private readonly ISeguroRepository _repository;

    public PesquisarSeguroHandler(ISeguroRepository repository)
    {
        _repository = repository;
    }

    public async Task<PesquisarSeguroResponse?> ExecutarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var seguro = await _repository.ObterPorIdAsync(id, cancellationToken);
        if (seguro is null) return null;

        return new PesquisarSeguroResponse(
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
    }
}

