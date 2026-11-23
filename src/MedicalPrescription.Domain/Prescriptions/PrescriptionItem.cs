using MedicalPrescription.Domain.Common;
using MedicalPrescription.Domain.Medications;

namespace MedicalPrescription.Domain.Prescriptions;

public sealed class PrescriptionItem : Entity
{
    private const string Rule = PrescriptionRules.StructuredPrescription;

    public int PrescriptionId { get; private set; }
    public int? MedicationId { get; private set; }
    public string MedicationName { get; private set; } = string.Empty;
    public string PharmaceuticalForm { get; private set; } = string.Empty;
    public decimal Dose { get; private set; }
    public string DoseUnit { get; private set; } = string.Empty;
    public string Frequency { get; private set; } = string.Empty;
    public DateOnly StartDate { get; private set; }
    public int? DurationDays { get; private set; }
    public bool ContinuousUse { get; private set; }
    public string AdministrationRoute { get; private set; } = string.Empty;
    public DateOnly? UsageDeadline { get; private set; }
    public string? Notes { get; private set; }

    public bool IsCatalogued => MedicationId.HasValue;

    private PrescriptionItem() { }

    public static PrescriptionItem FromCatalog(Medication medication, PrescriptionItemDetails details)
    {
        ArgumentNullException.ThrowIfNull(medication);

        var form = string.IsNullOrWhiteSpace(details.PharmaceuticalForm)
            ? medication.PharmaceuticalForm
            : details.PharmaceuticalForm;

        return Build(medication.Id, medication.DisplayName, details with { PharmaceuticalForm = form });
    }

    public static PrescriptionItem NonCatalogued(string? medicationName, PrescriptionItemDetails details) =>
        Build(null, medicationName, details);

    private static PrescriptionItem Build(int? medicationId, string? medicationName, PrescriptionItemDetails details)
    {
        if (details.Dose <= 0)
            throw new DomainException(Rule, "A dose deve ser maior que zero.");

        if (details.ContinuousUse && details.DurationDays.HasValue)
            throw new DomainException(Rule, "Medicamento de uso contínuo não deve possuir duração de tratamento.");

        if (!details.ContinuousUse && details.DurationDays is null or <= 0)
            throw new DomainException(Rule, "Informe a duração do tratamento em dias ou marque como uso contínuo.");

        if (details.DurationDays > PrescriptionRules.MaxDurationDays)
            throw new DomainException(Rule, $"A duração do tratamento não pode exceder {PrescriptionRules.MaxDurationDays} dias.");

        if (details.UsageDeadline.HasValue && details.UsageDeadline.Value < details.StartDate)
            throw new DomainException(Rule, "O prazo de utilização não pode ser anterior ao início do tratamento.");

        return new PrescriptionItem
        {
            MedicationId = medicationId,
            MedicationName = Guard.Required(medicationName, 200, Rule, "Nome do medicamento/princípio ativo"),
            PharmaceuticalForm = Guard.Required(details.PharmaceuticalForm, 50, Rule, "Forma farmacêutica"),
            Dose = details.Dose,
            DoseUnit = Guard.Required(details.DoseUnit, 20, Rule, "Unidade de medida da dose"),
            Frequency = Guard.Required(details.Frequency, 60, Rule, "Frequência"),
            StartDate = details.StartDate,
            DurationDays = details.DurationDays,
            ContinuousUse = details.ContinuousUse,
            AdministrationRoute = Guard.Required(details.AdministrationRoute, 40, Rule, "Via de administração"),
            UsageDeadline = details.UsageDeadline,
            Notes = Guard.Optional(details.Notes, 1000, Rule, "Observação")
        };
    }
}
