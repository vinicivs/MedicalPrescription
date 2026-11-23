using MedicalPrescription.Application.Abstractions;
using MedicalPrescription.Application.Common.Exceptions;
using MedicalPrescription.Application.Prescriptions.Contracts;

namespace MedicalPrescription.Application.Prescriptions.UseCases;

public sealed class GetPrescriptionById(IPrescriptionQueries queries)
{
    public async Task<PrescriptionDto> ExecuteAsync(int id, CancellationToken cancellationToken) =>
        await queries.GetByIdAsync(id, cancellationToken)
        ?? throw new NotFoundException("prescrição", id);
}
