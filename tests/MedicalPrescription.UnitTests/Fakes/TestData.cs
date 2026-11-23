using System.Reflection;
using MedicalPrescription.Domain.Common;
using MedicalPrescription.Domain.Medications;
using MedicalPrescription.Domain.Prescriptions;

namespace MedicalPrescription.UnitTests.Fakes;

internal static class TestData
{
    public static readonly DateOnly Today = new(2026, 1, 10);

    public static Prescriber Prescriber() => MedicalPrescription.Domain.Prescriptions.Prescriber.Create("Dra. Ana Souza", "CRM", "123456", "SP");

    public static PrescriptionItemDetails Details(
        decimal dose = 500,
        int? durationDays = 7,
        bool continuousUse = false,
        DateOnly? usageDeadline = null) =>
        new("Comprimido", dose, "mg", "8 em 8 horas", Today, durationDays, continuousUse, "Oral", usageDeadline, null);

    public static Medication Medication(int id, string name = "Dipirona", string concentration = "500 mg")
    {
        var medication = new Medication(name, name, "Comprimido", concentration);
        WithId(medication, id);
        return medication;
    }

    public static T WithId<T>(T entity, int id) where T : Entity
    {
        typeof(Entity).GetProperty(nameof(Entity.Id), BindingFlags.Instance | BindingFlags.Public)!
            .SetValue(entity, id);
        return entity;
    }
}
