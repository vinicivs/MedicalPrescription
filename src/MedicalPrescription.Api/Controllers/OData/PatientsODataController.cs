using MedicalPrescription.Application.Abstractions;
using MedicalPrescription.Application.Catalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace MedicalPrescription.Api.Controllers.OData;

public sealed class PatientsODataController(ICatalogQueries catalog) : ODataController
{
    [HttpGet("odata/Patients")]
    [EnableQuery(PageSize = ODataDefaults.PageSize, MaxTop = ODataDefaults.MaxTop)]
    public IQueryable<PatientDto> Get() => catalog.Patients();

    [HttpGet("odata/Patients({key})")]
    [HttpGet("odata/Patients/{key}")]
    [EnableQuery]
    public SingleResult<PatientDto> Get(int key) =>
        SingleResult.Create(catalog.Patients().Where(p => p.Id == key));
}
