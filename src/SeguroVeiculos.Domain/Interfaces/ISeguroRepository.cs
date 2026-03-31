using SeguroVeiculos.Domain.Entities;

namespace SeguroVeiculos.Domain.Interfaces;

public interface ISeguroRepository
{
    Task<Seguro> AdicionarAsync(Seguro seguro, CancellationToken cancellationToken = default);
    Task<Seguro?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Seguro>> ListarTodosAsync(CancellationToken cancellationToken = default);
}

