using MedicalPrescription.Application.Abstractions;
using MedicalPrescription.Application.Prescriptions.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace MedicalPrescription.Api.Controllers.OData;

public sealed class PrescriptionsODataController(IPrescriptionQueries queries) : ODataController
{
    [HttpGet("odata/Prescriptions")]
    [EnableQuery(PageSize = ODataDefaults.PageSize, MaxTop = ODataDefaults.MaxTop, MaxExpansionDepth = 2)]
    public IQueryable<PrescriptionDto> Get() => queries.Query();

    [HttpGet("odata/Prescriptions({key})")]
    [HttpGet("odata/Prescriptions/{key}")]
    [EnableQuery(MaxExpansionDepth = 2)]
    public SingleResult<PrescriptionDto> Get(int key) =>
        SingleResult.Create(queries.Query().Where(p => p.Id == key));
}
