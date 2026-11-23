namespace MedicalPrescription.Application.Common.Exceptions;

public sealed class NotFoundException(string resource, object key)
    : Exception($"Registro de {resource} com id {key} não encontrado.");
