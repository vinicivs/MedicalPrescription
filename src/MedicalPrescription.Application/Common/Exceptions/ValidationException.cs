namespace MedicalPrescription.Application.Common.Exceptions;

public sealed class ValidationException(IReadOnlyDictionary<string, string[]> errors)
    : Exception("Um ou mais campos são inválidos.")
{
    public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;

    public ValidationException(string field, string error)
        : this(new Dictionary<string, string[]> { [field] = [error] })
    {
    }
}
