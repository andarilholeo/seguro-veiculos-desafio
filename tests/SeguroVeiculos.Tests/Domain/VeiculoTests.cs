using SeguroVeiculos.Domain.Entities;
using Xunit;

namespace SeguroVeiculos.Tests.Domain;

public class VeiculoTests
{
    [Fact]
    public void CriarVeiculo_ComDadosValidos_CriaCorretamente()
    {
        var veiculo = new Veiculo(10_000m, "Fiat Uno");
        Assert.Equal(10_000m, veiculo.Valor);
        Assert.Equal("Fiat Uno", veiculo.MarcaModelo);
        Assert.NotEqual(Guid.Empty, veiculo.Id);
    }

    [Fact]
    public void CriarVeiculo_ValorZero_LancaExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Veiculo(0m, "Fiat Uno"));
    }

    [Fact]
    public void CriarVeiculo_ValorNegativo_LancaExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Veiculo(-1m, "Fiat Uno"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void CriarVeiculo_MarcaModeloVazio_LancaExcecao(string marcaModelo)
    {
        Assert.Throws<ArgumentException>(() => new Veiculo(10_000m, marcaModelo));
    }
}

