using MedicalPrescription.Application.Abstractions;
using MedicalPrescription.Application.Catalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace MedicalPrescription.Api.Controllers.OData;

public sealed class MedicationsODataController(ICatalogQueries catalog) : ODataController
{
    [HttpGet("odata/Medications")]
    [EnableQuery(PageSize = ODataDefaults.PageSize, MaxTop = ODataDefaults.MaxTop)]
    public IQueryable<MedicationDto> Get() => catalog.Medications();

    [HttpGet("odata/Medications({key})")]
    [HttpGet("odata/Medications/{key}")]
    [EnableQuery]
    public SingleResult<MedicationDto> Get(int key) =>
        SingleResult.Create(catalog.Medications().Where(m => m.Id == key));
}
