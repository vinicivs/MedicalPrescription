using MedicalPrescription.Application.Abstractions;
using MedicalPrescription.Domain.Medications;
using MedicalPrescription.Domain.Prescriptions;

namespace MedicalPrescription.UnitTests.Fakes;

internal sealed class InMemoryPatientRepository(params int[] existingIds) : IPatientRepository
{
    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken) =>
        Task.FromResult(existingIds.Contains(id));
}

internal sealed class InMemoryMedicationRepository(params Medication[] medications) : IMedicationRepository
{
    public Task<IReadOnlyDictionary<int, Medication>> GetByIdsAsync(IReadOnlyCollection<int> ids, CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyDictionary<int, Medication>>(
            medications.Where(m => ids.Contains(m.Id)).ToDictionary(m => m.Id));
}

internal sealed class InMemoryPrescriptionRepository : IPrescriptionRepository, IUnitOfWork
{
    private int _nextId = 1;

    public List<Prescription> Items { get; } = [];
    public int SaveCount { get; private set; }

    public Task<Prescription?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        Task.FromResult(Items.FirstOrDefault(p => p.Id == id));

    public Task AddAsync(Prescription prescription, CancellationToken cancellationToken)
    {
        Items.Add(TestData.WithId(prescription, _nextId++));
        return Task.CompletedTask;
    }

    public void Remove(Prescription prescription) => Items.Remove(prescription);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        SaveCount++;
        return Task.FromResult(1);
    }
}

internal sealed class FixedTimeProvider(DateTimeOffset now) : TimeProvider
{
    public override DateTimeOffset GetUtcNow() => now;
}
