using MedicalPrescription.Domain.Medications;

namespace MedicalPrescription.Application.Abstractions;

public interface IMedicationRepository
{
    Task<IReadOnlyDictionary<int, Medication>> GetByIdsAsync(IReadOnlyCollection<int> ids, CancellationToken cancellationToken);
}
