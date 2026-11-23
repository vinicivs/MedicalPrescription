using MedicalPrescription.Application.Abstractions;
using MedicalPrescription.Domain.Prescriptions;
using Microsoft.EntityFrameworkCore;

namespace MedicalPrescription.Infrastructure.Persistence.Repositories;

internal sealed class PrescriptionRepository(AppDbContext context) : IPrescriptionRepository
{
    public Task<Prescription?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        context.Prescriptions
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task AddAsync(Prescription prescription, CancellationToken cancellationToken) =>
        await context.Prescriptions.AddAsync(prescription, cancellationToken);

    public void Remove(Prescription prescription) => context.Prescriptions.Remove(prescription);
}
