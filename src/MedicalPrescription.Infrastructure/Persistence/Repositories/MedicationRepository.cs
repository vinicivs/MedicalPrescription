using MedicalPrescription.Application.Abstractions;
using MedicalPrescription.Domain.Medications;
using Microsoft.EntityFrameworkCore;

namespace MedicalPrescription.Infrastructure.Persistence.Repositories;

internal sealed class MedicationRepository(AppDbContext context) : IMedicationRepository
{
    public async Task<IReadOnlyDictionary<int, Medication>> GetByIdsAsync(
        IReadOnlyCollection<int> ids,
        CancellationToken cancellationToken) =>
        await context.Medications
            .AsNoTracking()
            .Where(m => ids.Contains(m.Id))
            .ToDictionaryAsync(m => m.Id, cancellationToken);
}
