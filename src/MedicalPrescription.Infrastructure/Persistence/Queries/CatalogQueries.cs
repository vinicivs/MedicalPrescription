using MedicalPrescription.Application.Abstractions;
using MedicalPrescription.Application.Catalog;
using Microsoft.EntityFrameworkCore;

namespace MedicalPrescription.Infrastructure.Persistence.Queries;

internal sealed class CatalogQueries(AppDbContext context) : ICatalogQueries
{
    public IQueryable<PatientDto> Patients() =>
        context.Patients.AsNoTracking().Select(p => new PatientDto
        {
            Id = p.Id,
            FullName = p.FullName,
            Cpf = p.Cpf,
            BirthDate = p.BirthDate,
            Sex = p.Sex
        });

    public IQueryable<MedicationDto> Medications() =>
        context.Medications.AsNoTracking().Select(m => new MedicationDto
        {
            Id = m.Id,
            Name = m.Name,
            ActiveIngredient = m.ActiveIngredient,
            PharmaceuticalForm = m.PharmaceuticalForm,
            Concentration = m.Concentration
        });
}
