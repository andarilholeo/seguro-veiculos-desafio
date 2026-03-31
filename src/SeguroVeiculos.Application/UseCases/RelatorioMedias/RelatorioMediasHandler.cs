using SeguroVeiculos.Domain.Common;
using SeguroVeiculos.Domain.Interfaces;

namespace SeguroVeiculos.Application.UseCases.RelatorioMedias;

public class RelatorioMediasHandler
{
    private readonly ISeguroRepository _repository;

    public RelatorioMediasHandler(ISeguroRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<RelatorioMediasResponse>> ExecutarAsync(CancellationToken cancellationToken = default)
    {
        var seguros = (await _repository.ListarTodosAsync(cancellationToken)).ToList();

        if (seguros.Count == 0)
            return Result<RelatorioMediasResponse>.Ok(new RelatorioMediasResponse(0, 0, 0, 0, 0, 0));

        var total = seguros.Count;
        var mediaValorVeiculo    = Math.Round(seguros.Average(s => s.Veiculo.Valor), 2);
        var mediaTaxaRisco       = Math.Round(seguros.Average(s => s.Calculo.TaxaRisco), 4);
        var mediaPremioRisco     = Math.Round(seguros.Average(s => s.Calculo.PremioRisco), 2);
        var mediaPremioPuro      = Math.Round(seguros.Average(s => s.Calculo.PremioPuro), 2);
        var mediaPremioComercial = Math.Round(seguros.Average(s => s.Calculo.PremioComercial), 2);

        return Result<RelatorioMediasResponse>.Ok(new RelatorioMediasResponse(
            total,
            mediaValorVeiculo,
            mediaTaxaRisco,
            mediaPremioRisco,
            mediaPremioPuro,
            mediaPremioComercial
        ));
    }
}

