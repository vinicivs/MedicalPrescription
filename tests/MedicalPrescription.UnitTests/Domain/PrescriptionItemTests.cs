using MedicalPrescription.Domain.Common;
using MedicalPrescription.Domain.Prescriptions;
using MedicalPrescription.UnitTests.Fakes;

namespace MedicalPrescription.UnitTests.Domain;

public class PrescriptionItemTests
{
    [Fact]
    public void FromCatalog_uses_medication_name_and_default_form()
    {
        var medication = TestData.Medication(10, "Losartana", "50 mg");

        var item = PrescriptionItem.FromCatalog(medication, TestData.Details() with { PharmaceuticalForm = null });

        Assert.Equal(10, item.MedicationId);
        Assert.Equal("Losartana 50 mg", item.MedicationName);
        Assert.Equal("Comprimido", item.PharmaceuticalForm);
        Assert.True(item.IsCatalogued);
    }

    [Fact]
    public void NonCatalogued_requires_medication_name()
    {
        var ex = Assert.Throws<DomainException>(() => PrescriptionItem.NonCatalogued(" ", TestData.Details()));
        Assert.Equal(PrescriptionRules.StructuredPrescription, ex.Code);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Dose_must_be_positive(decimal dose)
    {
        Assert.Throws<DomainException>(() => PrescriptionItem.NonCatalogued("Xarope", TestData.Details(dose: dose)));
    }

    [Fact]
    public void Continuous_use_cannot_have_duration()
    {
        Assert.Throws<DomainException>(() =>
            PrescriptionItem.NonCatalogued("Losartana", TestData.Details(durationDays: 30, continuousUse: true)));
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(366)]
    public void Non_continuous_use_requires_valid_duration(int? durationDays)
    {
        Assert.Throws<DomainException>(() =>
            PrescriptionItem.NonCatalogued("Amoxicilina", TestData.Details(durationDays: durationDays)));
    }

    [Fact]
    public void Continuous_use_without_duration_is_valid()
    {
        var item = PrescriptionItem.NonCatalogued("Losartana", TestData.Details(durationDays: null, continuousUse: true));

        Assert.True(item.ContinuousUse);
        Assert.Null(item.DurationDays);
    }

    [Fact]
    public void Usage_deadline_before_start_date_is_rejected()
    {
        Assert.Throws<DomainException>(() =>
            PrescriptionItem.NonCatalogued("Amoxicilina", TestData.Details(usageDeadline: TestData.Today.AddDays(-1))));
    }

    [Theory]
    [InlineData(null, "Comprimido", "mg", "8/8h", "Oral")]
    [InlineData("Amoxicilina", null, "mg", "8/8h", "Oral")]
    [InlineData("Amoxicilina", "Comprimido", null, "8/8h", "Oral")]
    [InlineData("Amoxicilina", "Comprimido", "mg", null, "Oral")]
    [InlineData("Amoxicilina", "Comprimido", "mg", "8/8h", null)]
    public void Mandatory_structured_fields_are_enforced(string? name, string? form, string? unit, string? frequency, string? route)
    {
        var details = new PrescriptionItemDetails(form, 1, unit, frequency, TestData.Today, 5, false, route, null, null);

        var ex = Assert.Throws<DomainException>(() => PrescriptionItem.NonCatalogued(name, details));
        Assert.Equal(PrescriptionRules.StructuredPrescription, ex.Code);
    }
}
