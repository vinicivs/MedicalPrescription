namespace MedicalPrescription.Domain.Prescriptions;

public sealed record PrescriptionItemDetails(
    string? PharmaceuticalForm,
    decimal Dose,
    string? DoseUnit,
    string? Frequency,
    DateOnly StartDate,
    int? DurationDays,
    bool ContinuousUse,
    string? AdministrationRoute,
    DateOnly? UsageDeadline,
    string? Notes);
