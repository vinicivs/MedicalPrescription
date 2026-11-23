using MedicalPrescription.Application.Prescriptions.Contracts;
using MedicalPrescription.Application.Prescriptions.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace MedicalPrescription.Api.Controllers;

[ApiController]
[Route("api/prescriptions")]
[Produces("application/json")]
public sealed class PrescriptionsController : ControllerBase
{
    [HttpGet("{id:int}", Name = nameof(GetPrescriptionById))]
    [ProducesResponseType<PrescriptionDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PrescriptionDto>> GetPrescriptionById(
        int id,
        [FromServices] GetPrescriptionById useCase,
        CancellationToken cancellationToken) =>
        Ok(await useCase.ExecuteAsync(id, cancellationToken));

    [HttpPost]
    [ProducesResponseType<PrescriptionDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<PrescriptionDto>> Create(
        SavePrescriptionRequest request,
        [FromServices] CreatePrescription createPrescription,
        [FromServices] GetPrescriptionById getPrescription,
        CancellationToken cancellationToken)
    {
        var id = await createPrescription.ExecuteAsync(request, cancellationToken);
        var created = await getPrescription.ExecuteAsync(id, cancellationToken);

        return CreatedAtRoute(nameof(GetPrescriptionById), new { id }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType<PrescriptionDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<PrescriptionDto>> Update(
        int id,
        SavePrescriptionRequest request,
        [FromServices] UpdatePrescription updatePrescription,
        [FromServices] GetPrescriptionById getPrescription,
        CancellationToken cancellationToken)
    {
        await updatePrescription.ExecuteAsync(id, request, cancellationToken);
        return Ok(await getPrescription.ExecuteAsync(id, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id,
        [FromServices] DeletePrescription deletePrescription,
        CancellationToken cancellationToken)
    {
        await deletePrescription.ExecuteAsync(id, cancellationToken);
        return NoContent();
    }
}
