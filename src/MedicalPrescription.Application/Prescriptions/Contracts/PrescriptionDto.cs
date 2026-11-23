using MedicalPrescription.Domain.Prescriptions;

namespace MedicalPrescription.Application.Prescriptions.Contracts;

public sealed class PrescriptionDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string PrescriberName { get; set; } = string.Empty;
    public string PrescriberCouncil { get; set; } = string.Empty;
    public string PrescriberCouncilNumber { get; set; } = string.Empty;
    public string PrescriberCouncilState { get; set; } = string.Empty;
    public PrescriptionType Type { get; set; }
    public string? FreeText { get; set; }
    public DateTime IssuedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int ItemCount { get; set; }
    public List<PrescriptionItemDto> Items { get; set; } = [];
}
