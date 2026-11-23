using MedicalPrescription.Application.Abstractions;
using MedicalPrescription.Application.Common.Exceptions;

namespace MedicalPrescription.Application.Prescriptions.UseCases;

public sealed class DeletePrescription(IPrescriptionRepository prescriptions, IUnitOfWork unitOfWork)
{
    public async Task ExecuteAsync(int id, CancellationToken cancellationToken)
    {
        var prescription = await prescriptions.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("prescrição", id);

        prescriptions.Remove(prescription);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
