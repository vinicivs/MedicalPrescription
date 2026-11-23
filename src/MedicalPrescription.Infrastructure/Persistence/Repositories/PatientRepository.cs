using MedicalPrescription.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MedicalPrescription.Infrastructure.Persistence.Repositories;

internal sealed class PatientRepository(AppDbContext context) : IPatientRepository
{
    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken) =>
        context.Patients.AnyAsync(p => p.Id == id, cancellationToken);
}
