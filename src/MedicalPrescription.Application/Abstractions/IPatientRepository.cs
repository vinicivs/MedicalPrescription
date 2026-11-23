namespace MedicalPrescription.Application.Abstractions;

public interface IPatientRepository
{
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
}
