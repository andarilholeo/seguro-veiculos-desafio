using SeguroVeiculos.Domain.Common;
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

    public Result<bool> Atualizar(decimal novoValorVeiculo, string novaMarcaModelo, string novoNome, string novoCpf, int novaIdade)
    {
        var resultadoVeiculo = Veiculo.Atualizar(novoValorVeiculo, novaMarcaModelo);
        if (!resultadoVeiculo.IsSuccess)
            return Result<bool>.Fail(resultadoVeiculo.Error!, resultadoVeiculo.ErrorType);

        var resultadoSegurado = Segurado.Atualizar(novoNome, novoCpf, novaIdade);
        if (!resultadoSegurado.IsSuccess)
            return Result<bool>.Fail(resultadoSegurado.Error!, resultadoSegurado.ErrorType);

        Calculo = CalculoSeguro.Calcular(novoValorVeiculo);
        return Result<bool>.Ok(true);
    }
}

