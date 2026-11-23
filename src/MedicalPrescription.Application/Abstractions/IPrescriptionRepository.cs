using MedicalPrescription.Domain.Prescriptions;

namespace MedicalPrescription.Application.Abstractions;

public interface IPrescriptionRepository
{
    Task<Prescription?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Prescription prescription, CancellationToken cancellationToken);
    void Remove(Prescription prescription);
}
