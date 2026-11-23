namespace MedicalPrescription.Domain.Common;

internal static class Guard
{
    public static string Required(string? value, int maxLength, string code, string fieldLabel)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(code, $"{fieldLabel} é obrigatório.");

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
            throw new DomainException(code, $"{fieldLabel} deve ter no máximo {maxLength} caracteres.");

        return trimmed;
    }

    public static string? Optional(string? value, int maxLength, string code, string fieldLabel)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
            throw new DomainException(code, $"{fieldLabel} deve ter no máximo {maxLength} caracteres.");

        return trimmed;
    }
}
