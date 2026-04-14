using SeguroVeiculos.Domain.Common;
using SeguroVeiculos.Domain.Interfaces;

namespace SeguroVeiculos.Application.UseCases.RemoverSeguro;

public class RemoverSeguroHandler
{
    private readonly ISeguroRepository _repository;

    public RemoverSeguroHandler(ISeguroRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> ExecutarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var seguro = await _repository.ObterPorIdAsync(id, cancellationToken);

        if (seguro is null)
            return Result<bool>.NotFound($"Seguro com id '{id}' não encontrado.");

        await _repository.RemoverAsync(seguro, cancellationToken);

        return Result<bool>.Ok(true);
    }
}
