using SeguroVeiculos.Application.Interfaces;
using SeguroVeiculos.Domain.Entities;
using SeguroVeiculos.Domain.Interfaces;

namespace SeguroVeiculos.Application.UseCases.CriarSeguro;

public class CriarSeguroHandler
{
    private readonly ISeguroRepository _repository;
    private readonly ISeguradoExternoService _seguradoExternoService;

    public CriarSeguroHandler(ISeguroRepository repository, ISeguradoExternoService seguradoExternoService)
    {
        _repository = repository;
        _seguradoExternoService = seguradoExternoService;
    }

    public async Task<CriarSeguroResponse> ExecutarAsync(CriarSeguroCommand command, CancellationToken cancellationToken = default)
    {
        // Tenta enriquecer dados do segurado via serviço externo (mock)
        var seguradoExterno = await _seguradoExternoService.ObterPorCpfAsync(command.CpfSegurado, cancellationToken);

        var nome = seguradoExterno?.Nome ?? command.NomeSegurado;
        var idade = seguradoExterno?.Idade ?? command.IdadeSegurado;

        var veiculo = new Veiculo(command.ValorVeiculo, command.MarcaModeloVeiculo);
        var segurado = new Segurado(nome, command.CpfSegurado, idade);
        var seguro = new Seguro(veiculo, segurado);

        await _repository.AdicionarAsync(seguro, cancellationToken);

        return new CriarSeguroResponse(
            seguro.Id,
            seguro.Veiculo.MarcaModelo,
            seguro.Veiculo.Valor,
            seguro.Segurado.Nome,
            seguro.Segurado.Cpf,
            seguro.Segurado.Idade,
            seguro.Calculo.TaxaRisco,
            seguro.Calculo.PremioRisco,
            seguro.Calculo.PremioPuro,
            seguro.Calculo.PremioComercial,
            seguro.CriadoEm
        );
    }
}

