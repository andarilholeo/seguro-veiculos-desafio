namespace SeguroVeiculos.Domain.ValueObjects;

/// <summary>
/// Value Object que encapsula o cálculo do seguro de veículo.
/// 
/// Fórmulas:
///   MARGEM_SEGURANÇA = 3%
///   LUCRO            = 5%
///   Taxa de Risco    = (ValorVeiculo * 5) / (2 * ValorVeiculo)  => constante 2.5%
///   Prêmio de Risco  = TaxaRisco * ValorVeiculo
///   Prêmio Puro      = PrêmioRisco * (1 + MARGEM_SEGURANÇA)
///   Prêmio Comercial = PrêmioPuro  * (1 + LUCRO)
/// </summary>
public class CalculoSeguro
{
    public const decimal MargemSeguranca = 0.03m;
    public const decimal Lucro = 0.05m;

    public decimal ValorVeiculo { get; private set; }
    public decimal TaxaRisco { get; private set; }
    public decimal PremioRisco { get; private set; }
    public decimal PremioPuro { get; private set; }
    public decimal PremioComercial { get; private set; }

    private CalculoSeguro() { }

    public static CalculoSeguro Calcular(decimal valorVeiculo)
    {
        if (valorVeiculo <= 0)
            throw new ArgumentException("O valor do veículo deve ser maior que zero.", nameof(valorVeiculo));

        // Taxa de Risco = (VV * 5) / (2 * VV) = 2.5% (constante didática)
        var taxaRisco = (valorVeiculo * 5m) / (2m * valorVeiculo);

        var premioRisco = taxaRisco / 100m * valorVeiculo;
        var premioPuro = premioRisco * (1m + MargemSeguranca);
        var premioComercial = premioPuro * (1m + Lucro);

        return new CalculoSeguro
        {
            ValorVeiculo = valorVeiculo,
            TaxaRisco = taxaRisco,
            PremioRisco = Math.Round(premioRisco, 2),
            PremioPuro = Math.Round(premioPuro, 2),
            PremioComercial = Math.Round(premioComercial, 2)
        };
    }
}

