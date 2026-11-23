using MedicalPrescription.Application.Prescriptions.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalPrescription.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);

        services.AddScoped<PrescriptionContentFactory>();
        services.AddScoped<GetPrescriptionById>();
        services.AddScoped<CreatePrescription>();
        services.AddScoped<UpdatePrescription>();
        services.AddScoped<DeletePrescription>();

        return services;
    }
}
