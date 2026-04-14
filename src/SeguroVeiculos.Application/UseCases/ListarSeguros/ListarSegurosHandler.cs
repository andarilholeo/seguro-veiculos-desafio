using SeguroVeiculos.Application.UseCases.PesquisarSeguro;
using SeguroVeiculos.Domain.Common;
using SeguroVeiculos.Domain.Interfaces;

namespace SeguroVeiculos.Application.UseCases.ListarSeguros;

public class ListarSegurosHandler
{
    private readonly ISeguroRepository _repository;

    public ListarSegurosHandler(ISeguroRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<PesquisarSeguroResponse>>> ExecutarAsync(CancellationToken cancellationToken = default)
    {
        var seguros = await _repository.ListarTodosAsync(cancellationToken);

        var response = seguros.Select(s => new PesquisarSeguroResponse(
            s.Id,
            s.Veiculo.MarcaModelo,
            s.Veiculo.Valor,
            s.Segurado.Nome,
            s.Segurado.Cpf,
            s.Segurado.Idade,
            s.Calculo.TaxaRisco,
            s.Calculo.PremioRisco,
            s.Calculo.PremioPuro,
            s.Calculo.PremioComercial,
            s.CriadoEm
        ));

        return Result<IEnumerable<PesquisarSeguroResponse>>.Ok(response);
    }
}
