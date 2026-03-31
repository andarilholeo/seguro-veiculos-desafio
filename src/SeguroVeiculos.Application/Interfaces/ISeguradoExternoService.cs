namespace SeguroVeiculos.Application.Interfaces;

public record SeguradoExternoDto(string Nome, string Cpf, int Idade);

public interface ISeguradoExternoService
{
    Task<SeguradoExternoDto?> ObterPorCpfAsync(string cpf, CancellationToken cancellationToken = default);
}

