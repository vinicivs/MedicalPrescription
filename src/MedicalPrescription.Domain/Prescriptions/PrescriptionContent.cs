using MedicalPrescription.Domain.Common;

namespace MedicalPrescription.Domain.Prescriptions;

public sealed class PrescriptionContent
{
    public PrescriptionType Type { get; }
    public string? FreeText { get; }
    public IReadOnlyList<PrescriptionItem> Items { get; }

    private PrescriptionContent(PrescriptionType type, string? freeText, IReadOnlyList<PrescriptionItem> items)
    {
        Type = type;
        FreeText = freeText;
        Items = items;
    }

    public static PrescriptionContent Unstructured(string? freeText)
    {
        var text = Guard.Required(freeText, PrescriptionRules.FreeTextMaxLength,
            PrescriptionRules.UnstructuredPrescription, "O texto da receita");

        return new PrescriptionContent(PrescriptionType.Unstructured, text, []);
    }

    public static PrescriptionContent Structured(IEnumerable<PrescriptionItem>? items)
    {
        var list = items?.ToList() ?? [];

        if (list.Count == 0)
            throw new DomainException(PrescriptionRules.StructuredPrescription,
                "A receita estruturada deve conter ao menos um medicamento.");

        var duplicated = list
            .Where(i => i.MedicationId.HasValue)
            .GroupBy(i => i.MedicationId)
            .FirstOrDefault(g => g.Count() > 1);

        if (duplicated is not null)
            throw new DomainException(PrescriptionRules.StructuredPrescription,
                $"O medicamento '{duplicated.First().MedicationName}' foi informado mais de uma vez.");

        return new PrescriptionContent(PrescriptionType.Structured, null, list);
    }
}
