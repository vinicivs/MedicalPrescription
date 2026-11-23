using MedicalPrescription.Application.Prescriptions.Contracts;

namespace MedicalPrescription.Application.Abstractions;

public interface IPrescriptionQueries
{
    IQueryable<PrescriptionDto> Query();
    Task<PrescriptionDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
