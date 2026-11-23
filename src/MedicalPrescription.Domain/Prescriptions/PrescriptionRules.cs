namespace MedicalPrescription.Domain.Prescriptions;

public static class PrescriptionRules
{
    public const string UnstructuredPrescription = "ECF.10.01";
    public const string StructuredPrescription = "ECF.10.03";
    public const string Prescriber = "PRESCRIBER";

    public const int FreeTextMaxLength = 4000;
    public const int MaxDurationDays = 365;
}
