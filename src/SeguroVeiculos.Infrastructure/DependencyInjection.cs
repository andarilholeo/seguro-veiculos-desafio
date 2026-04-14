using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeguroVeiculos.Application.Interfaces;
using SeguroVeiculos.Application.UseCases.AtualizarSeguro;
using SeguroVeiculos.Application.UseCases.CriarSeguro;
using SeguroVeiculos.Application.UseCases.ListarSeguros;
using SeguroVeiculos.Application.UseCases.PesquisarSeguro;
using SeguroVeiculos.Application.UseCases.RelatorioMedias;
using SeguroVeiculos.Application.UseCases.RemoverSeguro;
using SeguroVeiculos.Domain.Interfaces;
using SeguroVeiculos.Infrastructure.Data;
using SeguroVeiculos.Infrastructure.ExternalServices;
using SeguroVeiculos.Infrastructure.Repositories;

namespace SeguroVeiculos.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("SupabaseConnection") ??
            configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(
        connectionString,
        npgsqlOptions => npgsqlOptions
            .EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorCodesToAdd: null)
            .CommandTimeout(60)
        ));

        services.AddScoped<ISeguroRepository, SeguroRepository>();

        services.AddScoped<CriarSeguroHandler>();
        services.AddScoped<PesquisarSeguroHandler>();
        services.AddScoped<ListarSegurosHandler>();
        services.AddScoped<AtualizarSeguroHandler>();
        services.AddScoped<RemoverSeguroHandler>();
        services.AddScoped<RelatorioMediasHandler>();

        var seguradoUrl = configuration["SeguradoExternoUrl"] ?? "http://localhost:3001";
        services.AddHttpClient<ISeguradoExternoService, SeguradoExternoService>(client =>
        {
            client.BaseAddress = new Uri(seguradoUrl);
            client.Timeout = TimeSpan.FromSeconds(5);
        });

        return services;
    }
}

