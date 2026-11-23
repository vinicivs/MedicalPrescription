using MedicalPrescription.Application.Abstractions;
using MedicalPrescription.Infrastructure.Persistence;
using MedicalPrescription.Infrastructure.Persistence.Queries;
using MedicalPrescription.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalPrescription.Infrastructure;

public static class DependencyInjection
{
    private static readonly MySqlServerVersion ServerVersion = new(new Version(8, 4, 0));

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' não configurada.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion, mysql => mysql.EnableRetryOnFailure(5)));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IMedicationRepository, MedicationRepository>();
        services.AddScoped<IPrescriptionQueries, PrescriptionQueries>();
        services.AddScoped<ICatalogQueries, CatalogQueries>();

        services.AddHealthChecks().AddDbContextCheck<AppDbContext>("mysql", tags: ["ready"]);

        return services;
    }
}
