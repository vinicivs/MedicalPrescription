using MedicalPrescription.Domain.Prescriptions;

namespace MedicalPrescription.Application.Prescriptions.Contracts;

public sealed record SavePrescriptionRequest
{
    public int PatientId { get; init; }
    public string? PrescriberName { get; init; }
    public string? PrescriberCouncil { get; init; }
    public string? PrescriberCouncilNumber { get; init; }
    public string? PrescriberCouncilState { get; init; }
    public PrescriptionType Type { get; init; }
    public string? FreeText { get; init; }
    public IReadOnlyList<PrescriptionItemRequest> Items { get; init; } = [];
}

public sealed record PrescriptionItemRequest
{
    public int? MedicationId { get; init; }
    public string? MedicationName { get; init; }
    public string? PharmaceuticalForm { get; init; }
    public decimal Dose { get; init; }
    public string? DoseUnit { get; init; }
    public string? Frequency { get; init; }
    public DateOnly StartDate { get; init; }
    public int? DurationDays { get; init; }
    public bool ContinuousUse { get; init; }
    public string? AdministrationRoute { get; init; }
    public DateOnly? UsageDeadline { get; init; }
    public string? Notes { get; init; }
}
