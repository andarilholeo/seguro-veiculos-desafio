using SeguroVeiculos.Domain.Common;

namespace SeguroVeiculos.Domain.Entities;

public class Veiculo
{
    public Guid Id { get; private set; }
    public decimal Valor { get; private set; }
    public string MarcaModelo { get; private set; } = string.Empty;

    private Veiculo() { }

    public Veiculo(decimal valor, string marcaModelo)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do veículo deve ser maior que zero.", nameof(valor));
        if (string.IsNullOrWhiteSpace(marcaModelo))
            throw new ArgumentException("A marca/modelo do veículo é obrigatória.", nameof(marcaModelo));

        Id = Guid.NewGuid();
        Valor = valor;
        MarcaModelo = marcaModelo;
    }

    public Result<Veiculo> Atualizar(decimal valor, string marcaModelo)
    {
        if (valor <= 0)
            return Result<Veiculo>.Validation("O valor do veículo deve ser maior que zero.");
        if (string.IsNullOrWhiteSpace(marcaModelo))
            return Result<Veiculo>.Validation("A marca/modelo do veículo é obrigatória.");

        Valor = valor;
        MarcaModelo = marcaModelo;
        return Result<Veiculo>.Ok(this);
    }
}

