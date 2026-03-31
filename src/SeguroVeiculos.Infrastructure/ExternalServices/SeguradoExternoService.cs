using System.Text.Json;
using SeguroVeiculos.Application.Interfaces;

namespace SeguroVeiculos.Infrastructure.ExternalServices;

/// <summary>
/// Serviço que consulta dados do segurado em um mock REST (JSON Server).
/// Configure a URL base em appsettings.json: "SeguradoExternoUrl"
/// </summary>
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
            var response = await _httpClient.GetAsync($"/segurados?cpf={cpfLimpo}", cancellationToken);

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var lista = JsonSerializer.Deserialize<List<SeguradoExternoDto>>(json, _jsonOptions);
            return lista?.FirstOrDefault();
        }
        catch
        {
            // Serviço externo indisponível — fallback para dados do command
            return null;
        }
    }
}

