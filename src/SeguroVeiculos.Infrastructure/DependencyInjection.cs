using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeguroVeiculos.Application.Interfaces;
using SeguroVeiculos.Application.UseCases.CriarSeguro;
using SeguroVeiculos.Application.UseCases.PesquisarSeguro;
using SeguroVeiculos.Application.UseCases.RelatorioMedias;
using SeguroVeiculos.Domain.Interfaces;
using SeguroVeiculos.Infrastructure.Data;
using SeguroVeiculos.Infrastructure.ExternalServices;
using SeguroVeiculos.Infrastructure.Repositories;

namespace SeguroVeiculos.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // PostgreSQL + EF Core
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Repositórios
        services.AddScoped<ISeguroRepository, SeguroRepository>();

        // Use Cases (Application)
        services.AddScoped<CriarSeguroHandler>();
        services.AddScoped<PesquisarSeguroHandler>();
        services.AddScoped<RelatorioMediasHandler>();

        // Serviço externo de segurado (mock JSON Server)
        var seguradoUrl = configuration["SeguradoExternoUrl"] ?? "http://localhost:3001";
        services.AddHttpClient<ISeguradoExternoService, SeguradoExternoService>(client =>
        {
            client.BaseAddress = new Uri(seguradoUrl);
            client.Timeout = TimeSpan.FromSeconds(5);
        });

        return services;
    }
}

