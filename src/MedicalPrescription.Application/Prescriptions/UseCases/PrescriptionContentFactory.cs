using MedicalPrescription.Application.Abstractions;
using MedicalPrescription.Application.Common.Exceptions;
using MedicalPrescription.Application.Prescriptions.Contracts;
using MedicalPrescription.Domain.Medications;
using MedicalPrescription.Domain.Prescriptions;

namespace MedicalPrescription.Application.Prescriptions.UseCases;

public sealed class PrescriptionContentFactory(
    IPatientRepository patients,
    IMedicationRepository medications)
{
    public async Task<(Prescriber Prescriber, PrescriptionContent Content)> BuildAsync(
        SavePrescriptionRequest request,
        CancellationToken cancellationToken)
    {
        if (!await patients.ExistsAsync(request.PatientId, cancellationToken))
            throw new ValidationException(nameof(request.PatientId), "Paciente não encontrado.");

        var prescriber = Prescriber.Create(
            request.PrescriberName,
            request.PrescriberCouncil,
            request.PrescriberCouncilNumber,
            request.PrescriberCouncilState);

        var content = request.Type switch
        {
            PrescriptionType.Unstructured => PrescriptionContent.Unstructured(request.FreeText),
            PrescriptionType.Structured => PrescriptionContent.Structured(await BuildItemsAsync(request.Items, cancellationToken)),
            _ => throw new ValidationException(nameof(request.Type), "Tipo de receita inválido.")
        };

        return (prescriber, content);
    }

    private async Task<IReadOnlyList<PrescriptionItem>> BuildItemsAsync(
        IReadOnlyList<PrescriptionItemRequest> items,
        CancellationToken cancellationToken)
    {
        var medicationIds = items
            .Where(i => i.MedicationId.HasValue)
            .Select(i => i.MedicationId!.Value)
            .Distinct()
            .ToArray();

        var catalog = medicationIds.Length == 0
            ? new Dictionary<int, Medication>()
            : await medications.GetByIdsAsync(medicationIds, cancellationToken);

        var missing = medicationIds.Where(id => !catalog.ContainsKey(id)).ToArray();
        if (missing.Length > 0)
            throw new ValidationException(nameof(SavePrescriptionRequest.Items),
                $"Medicamento(s) não cadastrado(s): {string.Join(", ", missing)}.");

        return items.Select(item =>
        {
            var details = new PrescriptionItemDetails(
                item.PharmaceuticalForm,
                item.Dose,
                item.DoseUnit,
                item.Frequency,
                item.StartDate,
                item.DurationDays,
                item.ContinuousUse,
                item.AdministrationRoute,
                item.UsageDeadline,
                item.Notes);

            return item.MedicationId is { } medicationId
                ? PrescriptionItem.FromCatalog(catalog[medicationId], details)
                : PrescriptionItem.NonCatalogued(item.MedicationName, details);
        }).ToList();
    }
}
