using SeguroVeiculos.Domain.ValueObjects;

namespace SeguroVeiculos.Domain.Entities;

public class Seguro
{
    public Guid Id { get; private set; }
    public Veiculo Veiculo { get; private set; } = null!;
    public Segurado Segurado { get; private set; } = null!;
    public CalculoSeguro Calculo { get; private set; } = null!;
    public DateTime CriadoEm { get; private set; }

    private Seguro() { }

    public Seguro(Veiculo veiculo, Segurado segurado)
    {
        ArgumentNullException.ThrowIfNull(veiculo);
        ArgumentNullException.ThrowIfNull(segurado);

        Id = Guid.NewGuid();
        Veiculo = veiculo;
        Segurado = segurado;
        Calculo = CalculoSeguro.Calcular(veiculo.Valor);
        CriadoEm = DateTime.UtcNow;
    }
}

