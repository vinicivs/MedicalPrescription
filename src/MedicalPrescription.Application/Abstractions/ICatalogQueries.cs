using MedicalPrescription.Application.Catalog;

namespace MedicalPrescription.Application.Abstractions;

public interface ICatalogQueries
{
    IQueryable<PatientDto> Patients();
    IQueryable<MedicationDto> Medications();
}
