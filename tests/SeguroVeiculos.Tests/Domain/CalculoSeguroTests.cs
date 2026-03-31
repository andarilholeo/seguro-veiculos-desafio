using SeguroVeiculos.Domain.ValueObjects;
using Xunit;

namespace SeguroVeiculos.Tests.Domain;

public class CalculoSeguroTests
{
    // -------------------------------------------------------
    // Exemplo da especificação: Valor = R$ 10.000,00
    //   Taxa de Risco    = 2,5%
    //   Prêmio de Risco  = R$ 250,00
    //   Prêmio Puro      = R$ 257,50
    //   Prêmio Comercial = R$ 270,37 (arredondado de 270,375)
    // -------------------------------------------------------

    [Fact]
    public void Calcular_ValorDez_RetornaTaxaRiscoDoisPontoCinco()
    {
        var calculo = CalculoSeguro.Calcular(10_000m);
        Assert.Equal(2.5m, calculo.TaxaRisco);
    }

    [Fact]
    public void Calcular_ValorDez_RetornaPremioRiscoDuzentosECinquenta()
    {
        var calculo = CalculoSeguro.Calcular(10_000m);
        Assert.Equal(250m, calculo.PremioRisco);
    }

    [Fact]
    public void Calcular_ValorDez_RetornaPremioPuroCorreto()
    {
        // 250 * (1 + 0,03) = 257,50
        var calculo = CalculoSeguro.Calcular(10_000m);
        Assert.Equal(257.50m, calculo.PremioPuro);
    }

    [Fact]
    public void Calcular_ValorDez_RetornaPremioComercialCorreto()
    {
        // 257,50 * (1 + 0,05) = 270,375 → arredondado para 270,38
        var calculo = CalculoSeguro.Calcular(10_000m);
        Assert.Equal(270.38m, calculo.PremioComercial);
    }

    [Theory]
    [InlineData(10_000)]
    [InlineData(20_000)]
    [InlineData(50_000)]
    [InlineData(100_000)]
    public void Calcular_QualquerValor_TaxaRiscoSempreEhDoisPontoCinco(decimal valor)
    {
        // Taxa é constante pois simplifica para VV*5 / (2*VV) = 2,5
        var calculo = CalculoSeguro.Calcular(valor);
        Assert.Equal(2.5m, calculo.TaxaRisco);
    }

    [Theory]
    [InlineData(10_000, 250)]
    [InlineData(20_000, 500)]
    [InlineData(50_000, 1250)]
    public void Calcular_PremioRisco_EscalaLinearmenteComValorVeiculo(decimal valor, decimal premioEsperado)
    {
        var calculo = CalculoSeguro.Calcular(valor);
        Assert.Equal(premioEsperado, calculo.PremioRisco);
    }

    [Fact]
    public void Calcular_MargemSegurancaELucroSaoConstantes()
    {
        Assert.Equal(0.03m, CalculoSeguro.MargemSeguranca);
        Assert.Equal(0.05m, CalculoSeguro.Lucro);
    }

    [Fact]
    public void Calcular_ValorZero_LancaExcecao()
    {
        Assert.Throws<ArgumentException>(() => CalculoSeguro.Calcular(0m));
    }

    [Fact]
    public void Calcular_ValorNegativo_LancaExcecao()
    {
        Assert.Throws<ArgumentException>(() => CalculoSeguro.Calcular(-1000m));
    }

    [Fact]
    public void Calcular_PremioComercial_MaiorQuePremioPuro()
    {
        var calculo = CalculoSeguro.Calcular(10_000m);
        Assert.True(calculo.PremioComercial > calculo.PremioPuro);
    }

    [Fact]
    public void Calcular_PremioPuro_MaiorQuePremioRisco()
    {
        var calculo = CalculoSeguro.Calcular(10_000m);
        Assert.True(calculo.PremioPuro > calculo.PremioRisco);
    }
}

