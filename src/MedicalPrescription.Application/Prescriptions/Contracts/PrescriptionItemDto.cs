namespace MedicalPrescription.Application.Prescriptions.Contracts;

public sealed class PrescriptionItemDto
{
    public int Id { get; set; }
    public int? MedicationId { get; set; }
    public string MedicationName { get; set; } = string.Empty;
    public string PharmaceuticalForm { get; set; } = string.Empty;
    public decimal Dose { get; set; }
    public string DoseUnit { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public int? DurationDays { get; set; }
    public bool ContinuousUse { get; set; }
    public string AdministrationRoute { get; set; } = string.Empty;
    public DateOnly? UsageDeadline { get; set; }
    public string? Notes { get; set; }
}
