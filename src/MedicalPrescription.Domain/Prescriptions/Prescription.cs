using MedicalPrescription.Domain.Common;
using MedicalPrescription.Domain.Patients;

namespace MedicalPrescription.Domain.Prescriptions;

public sealed class Prescription : Entity
{
    private readonly List<PrescriptionItem> _items = [];

    public int PatientId { get; private set; }
    public Patient? Patient { get; private set; }
    public Prescriber Prescriber { get; private set; } = null!;
    public PrescriptionType Type { get; private set; }
    public string? FreeText { get; private set; }
    public DateTime IssuedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<PrescriptionItem> Items => _items.AsReadOnly();

    private Prescription() { }

    public static Prescription Issue(int patientId, Prescriber prescriber, PrescriptionContent content, DateTime issuedAt)
    {
        var prescription = new Prescription { IssuedAt = issuedAt };
        prescription.Apply(patientId, prescriber, content);
        return prescription;
    }

    public void Update(int patientId, Prescriber prescriber, PrescriptionContent content, DateTime updatedAt)
    {
        Apply(patientId, prescriber, content);
        UpdatedAt = updatedAt;
    }

    private void Apply(int patientId, Prescriber prescriber, PrescriptionContent content)
    {
        ArgumentNullException.ThrowIfNull(prescriber);
        ArgumentNullException.ThrowIfNull(content);

        if (patientId <= 0)
            throw new DomainException("PATIENT", "O paciente é obrigatório.");

        PatientId = patientId;
        Prescriber = prescriber;
        Type = content.Type;
        FreeText = content.FreeText;

        _items.Clear();
        _items.AddRange(content.Items);
    }
}
