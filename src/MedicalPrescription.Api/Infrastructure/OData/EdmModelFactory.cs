using MedicalPrescription.Application.Catalog;
using MedicalPrescription.Application.Prescriptions.Contracts;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace MedicalPrescription.Api.Infrastructure.OData;

public static class EdmModelFactory
{
    public static IEdmModel Create()
    {
        var builder = new ODataConventionModelBuilder();

        var prescriptions = builder.EntitySet<PrescriptionDto>("Prescriptions").EntityType;
        prescriptions.ContainsMany(p => p.Items);

        builder.EntitySet<PatientDto>("Patients");
        builder.EntitySet<MedicationDto>("Medications");

        return builder.GetEdmModel();
    }
}
