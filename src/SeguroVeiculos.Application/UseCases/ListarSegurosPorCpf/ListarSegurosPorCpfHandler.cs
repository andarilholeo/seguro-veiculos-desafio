using SeguroVeiculos.Application.UseCases.PesquisarSeguro;
using SeguroVeiculos.Domain.Common;
using SeguroVeiculos.Domain.Interfaces;

namespace SeguroVeiculos.Application.UseCases.ListarSegurosPorCpf;

public class ListarSegurosPorCpfHandler
{
    private readonly ISeguroRepository _repository;

    public ListarSegurosPorCpfHandler(ISeguroRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<PesquisarSeguroResponse>>> ExecutarAsync(string cpf, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return Result<IEnumerable<PesquisarSeguroResponse>>.Validation("O CPF do segurado é obrigatório.");

        var seguros = await _repository.ListarPorCpfAsync(cpf, cancellationToken);

        if (!seguros.Any())
            return Result<IEnumerable<PesquisarSeguroResponse>>.NotFound($"Nenhum seguro encontrado para o CPF '{cpf}'.");

        var response = seguros.Select(s => new PesquisarSeguroResponse(
            s.Id,
            s.Veiculo.MarcaModelo,
            s.Veiculo.Valor,
            s.Segurado.Nome,
            s.Segurado.Cpf,
            s.Segurado.Idade,
            s.Calculo.TaxaRisco,
            s.Calculo.PremioRisco,
            s.Calculo.PremioPuro,
            s.Calculo.PremioComercial,
            s.CriadoEm
        ));

        return Result<IEnumerable<PesquisarSeguroResponse>>.Ok(response);
    }
}
