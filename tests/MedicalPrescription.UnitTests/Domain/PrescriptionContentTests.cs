using MedicalPrescription.Domain.Common;
using MedicalPrescription.Domain.Prescriptions;
using MedicalPrescription.UnitTests.Fakes;

namespace MedicalPrescription.UnitTests.Domain;

public class PrescriptionContentTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Unstructured_without_text_violates_ECF_10_01(string? text)
    {
        var ex = Assert.Throws<DomainException>(() => PrescriptionContent.Unstructured(text));
        Assert.Equal(PrescriptionRules.UnstructuredPrescription, ex.Code);
    }

    [Fact]
    public void Unstructured_text_longer_than_limit_is_rejected()
    {
        var text = new string('a', PrescriptionRules.FreeTextMaxLength + 1);
        var ex = Assert.Throws<DomainException>(() => PrescriptionContent.Unstructured(text));
        Assert.Equal(PrescriptionRules.UnstructuredPrescription, ex.Code);
    }

    [Fact]
    public void Unstructured_keeps_trimmed_text_and_has_no_items()
    {
        var content = PrescriptionContent.Unstructured("  Amoxicilina 500mg, 1 cp de 8/8h por 7 dias  ");

        Assert.Equal(PrescriptionType.Unstructured, content.Type);
        Assert.Equal("Amoxicilina 500mg, 1 cp de 8/8h por 7 dias", content.FreeText);
        Assert.Empty(content.Items);
    }

    [Fact]
    public void Structured_without_items_violates_ECF_10_03()
    {
        var ex = Assert.Throws<DomainException>(() => PrescriptionContent.Structured([]));
        Assert.Equal(PrescriptionRules.StructuredPrescription, ex.Code);
    }

    [Fact]
    public void Structured_with_same_catalogued_medication_twice_is_rejected()
    {
        var medication = TestData.Medication(1);
        var items = new[]
        {
            PrescriptionItem.FromCatalog(medication, TestData.Details()),
            PrescriptionItem.FromCatalog(medication, TestData.Details())
        };

        var ex = Assert.Throws<DomainException>(() => PrescriptionContent.Structured(items));
        Assert.Equal(PrescriptionRules.StructuredPrescription, ex.Code);
    }

    [Fact]
    public void Structured_accepts_catalogued_and_non_catalogued_items()
    {
        var items = new[]
        {
            PrescriptionItem.FromCatalog(TestData.Medication(1), TestData.Details()),
            PrescriptionItem.NonCatalogued("Fórmula manipulada X", TestData.Details())
        };

        var content = PrescriptionContent.Structured(items);

        Assert.Equal(PrescriptionType.Structured, content.Type);
        Assert.Null(content.FreeText);
        Assert.Equal(2, content.Items.Count);
    }
}
