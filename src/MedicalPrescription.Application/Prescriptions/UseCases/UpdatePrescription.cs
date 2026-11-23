using MedicalPrescription.Application.Abstractions;
using MedicalPrescription.Application.Common.Exceptions;
using MedicalPrescription.Application.Prescriptions.Contracts;

namespace MedicalPrescription.Application.Prescriptions.UseCases;

public sealed class UpdatePrescription(
    PrescriptionContentFactory contentFactory,
    IPrescriptionRepository prescriptions,
    IUnitOfWork unitOfWork,
    TimeProvider clock)
{
    public async Task ExecuteAsync(int id, SavePrescriptionRequest request, CancellationToken cancellationToken)
    {
        var prescription = await prescriptions.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("prescrição", id);

        var (prescriber, content) = await contentFactory.BuildAsync(request, cancellationToken);

        prescription.Update(request.PatientId, prescriber, content, clock.GetUtcNow().UtcDateTime);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
