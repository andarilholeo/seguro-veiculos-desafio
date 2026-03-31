using SeguroVeiculos.Domain.Entities;
using Xunit;

namespace SeguroVeiculos.Tests.Domain;

public class SeguroTests
{
    [Fact]
    public void CriarSeguro_ComDadosValidos_CalculaAutomaticamente()
    {
        var veiculo = new Veiculo(10_000m, "Toyota Corolla");
        var segurado = new Segurado("João Silva", "123.456.789-00", 35);

        var seguro = new Seguro(veiculo, segurado);

        Assert.NotEqual(Guid.Empty, seguro.Id);
        Assert.NotNull(seguro.Calculo);
        Assert.Equal(270.38m, seguro.Calculo.PremioComercial);
        Assert.True(seguro.CriadoEm <= DateTime.UtcNow);
    }

    [Fact]
    public void CriarSeguro_VeiculoNulo_LancaExcecao()
    {
        var segurado = new Segurado("João Silva", "123.456.789-00", 35);
        Assert.Throws<ArgumentNullException>(() => new Seguro(null!, segurado));
    }

    [Fact]
    public void CriarSeguro_SeguradoNulo_LancaExcecao()
    {
        var veiculo = new Veiculo(10_000m, "Toyota Corolla");
        Assert.Throws<ArgumentNullException>(() => new Seguro(veiculo, null!));
    }
}

