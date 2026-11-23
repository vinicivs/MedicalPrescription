using MedicalPrescription.Domain.Common;

namespace MedicalPrescription.Domain.Medications;

public sealed class Medication : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string ActiveIngredient { get; private set; } = string.Empty;
    public string PharmaceuticalForm { get; private set; } = string.Empty;
    public string? Concentration { get; private set; }

    private Medication() { }

    public Medication(string name, string activeIngredient, string pharmaceuticalForm, string? concentration)
    {
        Name = name;
        ActiveIngredient = activeIngredient;
        PharmaceuticalForm = pharmaceuticalForm;
        Concentration = concentration;
    }

    public string DisplayName => string.IsNullOrWhiteSpace(Concentration) ? Name : $"{Name} {Concentration}";
}
