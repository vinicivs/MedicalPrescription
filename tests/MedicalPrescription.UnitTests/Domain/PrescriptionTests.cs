using MedicalPrescription.Domain.Common;
using MedicalPrescription.Domain.Prescriptions;
using MedicalPrescription.UnitTests.Fakes;

namespace MedicalPrescription.UnitTests.Domain;

public class PrescriptionTests
{
    private static readonly DateTime Now = new(2026, 1, 10, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Issue_sets_issue_date_and_content()
    {
        var prescription = Prescription.Issue(1, TestData.Prescriber(), PrescriptionContent.Unstructured("Repouso"), Now);

        Assert.Equal(1, prescription.PatientId);
        Assert.Equal(Now, prescription.IssuedAt);
        Assert.Null(prescription.UpdatedAt);
        Assert.Equal(PrescriptionType.Unstructured, prescription.Type);
    }

    [Fact]
    public void Issue_requires_patient()
    {
        Assert.Throws<DomainException>(() =>
            Prescription.Issue(0, TestData.Prescriber(), PrescriptionContent.Unstructured("Repouso"), Now));
    }

    [Fact]
    public void Update_switching_to_unstructured_clears_items()
    {
        var structured = PrescriptionContent.Structured([PrescriptionItem.NonCatalogued("Dipirona", TestData.Details())]);
        var prescription = Prescription.Issue(1, TestData.Prescriber(), structured, Now);

        prescription.Update(2, TestData.Prescriber(), PrescriptionContent.Unstructured("Texto livre"), Now.AddHours(1));

        Assert.Equal(2, prescription.PatientId);
        Assert.Equal(PrescriptionType.Unstructured, prescription.Type);
        Assert.Empty(prescription.Items);
        Assert.Equal("Texto livre", prescription.FreeText);
        Assert.Equal(Now.AddHours(1), prescription.UpdatedAt);
        Assert.Equal(Now, prescription.IssuedAt);
    }

    [Theory]
    [InlineData("S")]
    [InlineData("S1")]
    [InlineData("SPX")]
    public void Prescriber_council_state_must_have_two_letters(string state)
    {
        Assert.Throws<DomainException>(() => Prescriber.Create("Dr. João", "CRM", "1234", state));
    }

    [Fact]
    public void Prescriber_normalizes_council_and_state()
    {
        var prescriber = Prescriber.Create(" Dr. João ", "crm", "1234", "rj");

        Assert.Equal("Dr. João", prescriber.Name);
        Assert.Equal("CRM", prescriber.Council);
        Assert.Equal("RJ", prescriber.CouncilState);
    }
}
