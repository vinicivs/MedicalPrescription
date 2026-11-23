using MedicalPrescription.Application.Abstractions;
using MedicalPrescription.Application.Prescriptions.Contracts;
using MedicalPrescription.Domain.Prescriptions;

namespace MedicalPrescription.Application.Prescriptions.UseCases;

public sealed class CreatePrescription(
    PrescriptionContentFactory contentFactory,
    IPrescriptionRepository prescriptions,
    IUnitOfWork unitOfWork,
    TimeProvider clock)
{
    public async Task<int> ExecuteAsync(SavePrescriptionRequest request, CancellationToken cancellationToken)
    {
        var (prescriber, content) = await contentFactory.BuildAsync(request, cancellationToken);

        var prescription = Prescription.Issue(request.PatientId, prescriber, content, clock.GetUtcNow().UtcDateTime);

        await prescriptions.AddAsync(prescription, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return prescription.Id;
    }
}
