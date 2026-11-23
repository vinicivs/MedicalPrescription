using System.Linq.Expressions;
using MedicalPrescription.Application.Abstractions;
using MedicalPrescription.Application.Prescriptions.Contracts;
using MedicalPrescription.Domain.Prescriptions;
using Microsoft.EntityFrameworkCore;

namespace MedicalPrescription.Infrastructure.Persistence.Queries;

internal sealed class PrescriptionQueries(AppDbContext context) : IPrescriptionQueries
{
    private static readonly Expression<Func<Prescription, PrescriptionDto>> ToDto = p => new PrescriptionDto
    {
        Id = p.Id,
        PatientId = p.PatientId,
        PatientName = p.Patient!.FullName,
        PrescriberName = p.Prescriber.Name,
        PrescriberCouncil = p.Prescriber.Council,
        PrescriberCouncilNumber = p.Prescriber.CouncilNumber,
        PrescriberCouncilState = p.Prescriber.CouncilState,
        Type = p.Type,
        FreeText = p.FreeText,
        IssuedAt = p.IssuedAt,
        UpdatedAt = p.UpdatedAt,
        ItemCount = p.Items.Count,
        Items = p.Items
            .OrderBy(i => i.Id)
            .Select(i => new PrescriptionItemDto
            {
                Id = i.Id,
                MedicationId = i.MedicationId,
                MedicationName = i.MedicationName,
                PharmaceuticalForm = i.PharmaceuticalForm,
                Dose = i.Dose,
                DoseUnit = i.DoseUnit,
                Frequency = i.Frequency,
                StartDate = i.StartDate,
                DurationDays = i.DurationDays,
                ContinuousUse = i.ContinuousUse,
                AdministrationRoute = i.AdministrationRoute,
                UsageDeadline = i.UsageDeadline,
                Notes = i.Notes
            })
            .ToList()
    };

    public IQueryable<PrescriptionDto> Query() =>
        context.Prescriptions.AsNoTracking().Select(ToDto);

    public Task<PrescriptionDto?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        context.Prescriptions
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(ToDto)
            .FirstOrDefaultAsync(cancellationToken);
}
