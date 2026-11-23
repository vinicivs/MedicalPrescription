using MedicalPrescription.Domain.Patients;

namespace MedicalPrescription.Application.Catalog;

public sealed class PatientDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public Sex Sex { get; set; }
}
