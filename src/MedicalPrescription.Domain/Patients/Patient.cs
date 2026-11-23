using MedicalPrescription.Domain.Common;

namespace MedicalPrescription.Domain.Patients;

public sealed class Patient : Entity
{
    public string FullName { get; private set; } = string.Empty;
    public string Cpf { get; private set; } = string.Empty;
    public DateOnly BirthDate { get; private set; }
    public Sex Sex { get; private set; }

    private Patient() { }

    public Patient(string fullName, string cpf, DateOnly birthDate, Sex sex)
    {
        FullName = fullName;
        Cpf = cpf;
        BirthDate = birthDate;
        Sex = sex;
    }
}
