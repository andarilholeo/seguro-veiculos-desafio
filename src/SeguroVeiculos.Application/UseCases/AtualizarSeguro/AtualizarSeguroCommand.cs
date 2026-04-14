namespace SeguroVeiculos.Application.UseCases.AtualizarSeguro;

public record AtualizarSeguroCommand(
    decimal ValorVeiculo,
    string MarcaModeloVeiculo,
    string NomeSegurado,
    string CpfSegurado,
    int IdadeSegurado
);
