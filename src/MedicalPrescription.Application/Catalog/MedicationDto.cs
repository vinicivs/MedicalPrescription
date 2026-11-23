namespace MedicalPrescription.Application.Catalog;

public sealed class MedicationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ActiveIngredient { get; set; } = string.Empty;
    public string PharmaceuticalForm { get; set; } = string.Empty;
    public string? Concentration { get; set; }
}
