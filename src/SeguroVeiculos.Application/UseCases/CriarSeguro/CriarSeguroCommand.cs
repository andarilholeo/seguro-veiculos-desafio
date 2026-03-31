namespace SeguroVeiculos.Application.UseCases.CriarSeguro;

public record CriarSeguroCommand(
    decimal ValorVeiculo,
    string MarcaModeloVeiculo,
    string NomeSegurado,
    string CpfSegurado,
    int IdadeSegurado
);

