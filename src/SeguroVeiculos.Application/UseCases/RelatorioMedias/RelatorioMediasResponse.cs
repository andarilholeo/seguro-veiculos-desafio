namespace SeguroVeiculos.Application.UseCases.RelatorioMedias;

public record RelatorioMediasResponse(
    int TotalSeguros,
    decimal MediaValorVeiculo,
    decimal MediaTaxaRisco,
    decimal MediaPremioRisco,
    decimal MediaPremioPuro,
    decimal MediaPremioComercial
);

