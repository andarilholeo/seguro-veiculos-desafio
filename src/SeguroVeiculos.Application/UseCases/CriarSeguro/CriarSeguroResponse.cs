namespace SeguroVeiculos.Application.UseCases.CriarSeguro;

public record CriarSeguroResponse(
    Guid Id,
    string MarcaModeloVeiculo,
    decimal ValorVeiculo,
    string NomeSegurado,
    string CpfSegurado,
    int IdadeSegurado,
    decimal TaxaRisco,
    decimal PremioRisco,
    decimal PremioPuro,
    decimal PremioComercial,
    DateTime CriadoEm
);

