using Microsoft.EntityFrameworkCore;
using SeguroVeiculos.Domain.Entities;
using SeguroVeiculos.Domain.Interfaces;
using SeguroVeiculos.Infrastructure.Data;

namespace SeguroVeiculos.Infrastructure.Repositories;

public class SeguroRepository : ISeguroRepository
{
    private readonly AppDbContext _context;

    public SeguroRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Seguro> AdicionarAsync(Seguro seguro, CancellationToken cancellationToken = default)
    {
        _context.Seguros.Add(seguro);
        await _context.SaveChangesAsync(cancellationToken);
        return seguro;
    }

    public async Task<Seguro?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Seguros
            .Include(s => s.Veiculo)
            .Include(s => s.Segurado)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Seguro>> ListarTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Seguros
            .Include(s => s.Veiculo)
            .Include(s => s.Segurado)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Seguro>> ListarPorCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        return await _context.Seguros
            .Include(s => s.Veiculo)
            .Include(s => s.Segurado)
            .Where(s => s.Segurado.Cpf == cpf)
            .ToListAsync(cancellationToken);
    }

    public async Task<Seguro> AtualizarAsync(Seguro seguro, CancellationToken cancellationToken = default)
    {
        _context.Seguros.Update(seguro);
        await _context.SaveChangesAsync(cancellationToken);
        return seguro;
    }

    public async Task RemoverAsync(Seguro seguro, CancellationToken cancellationToken = default)
    {
        _context.Seguros.Remove(seguro);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

