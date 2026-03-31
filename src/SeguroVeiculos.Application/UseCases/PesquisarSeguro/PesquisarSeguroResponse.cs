namespace SeguroVeiculos.Application.UseCases.PesquisarSeguro;

public record PesquisarSeguroResponse(
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

