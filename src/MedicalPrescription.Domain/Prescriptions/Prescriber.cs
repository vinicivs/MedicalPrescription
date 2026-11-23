using MedicalPrescription.Domain.Common;

namespace MedicalPrescription.Domain.Prescriptions;

public sealed record Prescriber
{
    public string Name { get; private init; } = string.Empty;
    public string Council { get; private init; } = string.Empty;
    public string CouncilNumber { get; private init; } = string.Empty;
    public string CouncilState { get; private init; } = string.Empty;

    private Prescriber() { }

    public static Prescriber Create(string? name, string? council, string? councilNumber, string? councilState)
    {
        var state = Guard.Required(councilState, 2, PrescriptionRules.Prescriber, "UF do conselho");
        if (state.Length != 2 || !state.All(char.IsLetter))
            throw new DomainException(PrescriptionRules.Prescriber, "UF do conselho deve conter 2 letras.");

        return new Prescriber
        {
            Name = Guard.Required(name, 150, PrescriptionRules.Prescriber, "Nome do prescritor"),
            Council = Guard.Required(council, 10, PrescriptionRules.Prescriber, "Conselho profissional").ToUpperInvariant(),
            CouncilNumber = Guard.Required(councilNumber, 20, PrescriptionRules.Prescriber, "Número do conselho"),
            CouncilState = state.ToUpperInvariant()
        };
    }
}
