using System.Text.Json;
using SeguroVeiculos.Application.Interfaces;

namespace SeguroVeiculos.Infrastructure.ExternalServices;

public class SeguradoExternoService : ISeguradoExternoService
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public SeguradoExternoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SeguradoExternoDto?> ObterPorCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        try
        {
            var cpfLimpo = new string(cpf.Where(char.IsDigit).ToArray());
            var response = await _httpClient.GetAsync($"/dados-segurados?cpf={cpfLimpo}", cancellationToken);

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var lista = JsonSerializer.Deserialize<List<SeguradoExternoDto>>(json, _jsonOptions);
            return lista?.FirstOrDefault();
        }
        catch
        {
            return null;
        }
    }

    public async Task<IEnumerable<SeguradoExternoDto>> ObterPorNomeAsync(string nome, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/dados-segurados?nome={Uri.EscapeDataString(nome)}", cancellationToken);

            if (!response.IsSuccessStatusCode) return [];

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<List<SeguradoExternoDto>>(json, _jsonOptions) ?? [];
        }
        catch
        {
            return [];
        }
    }
}

