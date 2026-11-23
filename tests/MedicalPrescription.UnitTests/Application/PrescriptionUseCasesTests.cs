using MedicalPrescription.Application.Common.Exceptions;
using MedicalPrescription.Application.Prescriptions.Contracts;
using MedicalPrescription.Application.Prescriptions.UseCases;
using MedicalPrescription.Domain.Common;
using MedicalPrescription.Domain.Prescriptions;
using MedicalPrescription.UnitTests.Fakes;

namespace MedicalPrescription.UnitTests.Application;

public class PrescriptionUseCasesTests
{
    private static readonly DateTimeOffset Now = new(2026, 1, 10, 12, 0, 0, TimeSpan.Zero);

    private readonly InMemoryPrescriptionRepository _repository = new();
    private readonly PrescriptionContentFactory _factory = new(
        new InMemoryPatientRepository(1, 2),
        new InMemoryMedicationRepository(TestData.Medication(5, "Amoxicilina", "500 mg")));
    private readonly FixedTimeProvider _clock = new(Now);

    private CreatePrescription Create => new(_factory, _repository, _repository, _clock);
    private UpdatePrescription Update => new(_factory, _repository, _repository, _clock);
    private DeletePrescription Delete => new(_repository, _repository);

    private static SavePrescriptionRequest StructuredRequest(params PrescriptionItemRequest[] items) => new()
    {
        PatientId = 1,
        PrescriberName = "Dra. Ana Souza",
        PrescriberCouncil = "CRM",
        PrescriberCouncilNumber = "123456",
        PrescriberCouncilState = "SP",
        Type = PrescriptionType.Structured,
        Items = items
    };

    private static PrescriptionItemRequest Item(int? medicationId = 5, string? name = null) => new()
    {
        MedicationId = medicationId,
        MedicationName = name,
        Dose = 1,
        DoseUnit = "comprimido",
        Frequency = "8 em 8 horas",
        StartDate = TestData.Today,
        DurationDays = 7,
        AdministrationRoute = "Oral"
    };

    [Fact]
    public async Task Create_persists_structured_prescription_using_catalog_data()
    {
        var id = await Create.ExecuteAsync(StructuredRequest(Item(), Item(null, "Manipulado X") with { PharmaceuticalForm = "Cápsula" }), CancellationToken.None);

        var saved = Assert.Single(_repository.Items);
        Assert.Equal(id, saved.Id);
        Assert.Equal(Now.UtcDateTime, saved.IssuedAt);
        Assert.Equal(1, _repository.SaveCount);
        Assert.Collection(saved.Items,
            i => Assert.Equal("Amoxicilina 500 mg", i.MedicationName),
            i => Assert.Null(i.MedicationId));
    }

    [Fact]
    public async Task Create_with_unknown_patient_fails_validation()
    {
        var request = StructuredRequest(Item()) with { PatientId = 99 };

        var ex = await Assert.ThrowsAsync<ValidationException>(() => Create.ExecuteAsync(request, CancellationToken.None));

        Assert.Contains(nameof(SavePrescriptionRequest.PatientId), ex.Errors.Keys);
        Assert.Empty(_repository.Items);
    }

    [Fact]
    public async Task Create_with_unknown_medication_fails_validation()
    {
        await Assert.ThrowsAsync<ValidationException>(() =>
            Create.ExecuteAsync(StructuredRequest(Item(medicationId: 42)), CancellationToken.None));
    }

    [Fact]
    public async Task Create_unstructured_without_text_violates_business_rule()
    {
        var request = StructuredRequest() with { Type = PrescriptionType.Unstructured, FreeText = "" };

        var ex = await Assert.ThrowsAsync<DomainException>(() => Create.ExecuteAsync(request, CancellationToken.None));

        Assert.Equal(PrescriptionRules.UnstructuredPrescription, ex.Code);
        Assert.Equal(0, _repository.SaveCount);
    }

    [Fact]
    public async Task Update_replaces_content_and_sets_update_date()
    {
        var id = await Create.ExecuteAsync(StructuredRequest(Item()), CancellationToken.None);
        var request = StructuredRequest() with { PatientId = 2, Type = PrescriptionType.Unstructured, FreeText = "Repouso por 3 dias" };

        await Update.ExecuteAsync(id, request, CancellationToken.None);

        var saved = Assert.Single(_repository.Items);
        Assert.Equal(2, saved.PatientId);
        Assert.Empty(saved.Items);
        Assert.Equal(Now.UtcDateTime, saved.UpdatedAt);
    }

    [Fact]
    public async Task Update_of_missing_prescription_throws_not_found()
    {
        await Assert.ThrowsAsync<NotFoundException>(() =>
            Update.ExecuteAsync(123, StructuredRequest(Item()), CancellationToken.None));
    }

    [Fact]
    public async Task Delete_removes_prescription()
    {
        var id = await Create.ExecuteAsync(StructuredRequest(Item()), CancellationToken.None);

        await Delete.ExecuteAsync(id, CancellationToken.None);

        Assert.Empty(_repository.Items);
    }

    [Fact]
    public async Task Delete_of_missing_prescription_throws_not_found()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => Delete.ExecuteAsync(7, CancellationToken.None));
    }
}
