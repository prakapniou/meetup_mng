namespace MeetupManager.Web.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
//[Authorize(AuthenticationSchemes = "Bearer")] //may be without schema, it sets by di
public class SponsorController:ControllerBase
{
    private readonly IApiService<SponsorDto> _service;
    private readonly IMessageBrokerService _broker;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="broker"></param>
    public SponsorController(
        IApiService<SponsorDto> service,
        IMessageBrokerService broker)
    {
        _service= service;
        _broker=broker;
    }

    /// <summary>
    /// Get all of <see cref="Sponsor"/> collection.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Sponsor"/> collection.
    /// </returns>
    /// <response code="200">Returns the <see cref="Sponsor"/> collection</response>
    /// <response code="204">If <see cref="Sponsor"/> collection is empty.</response>
    /// <response code="404">If <see cref="Sponsor"/> collection not found.</response>
    /// <response code="500">If the generic error in the server.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SponsorDto>>> Get()
    {
        var dtos = await _service.GetAsync();
        _broker.SendMessage(dtos);
        return Ok(dtos);
    }

    /// <summary>Get <see cref="Sponsor"/> by <paramref name="id"/> identifier.</summary>
    /// <param name="id"></param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Sponsor"/> instance.
    /// </returns>
    /// <response code="200">Returns the <see cref="Sponsor"/> collection</response>
    /// <response code="404">If <see cref="Sponsor"/> instance not found.</response>
    /// <response code="500">If the generic error in the server.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<SponsorDto>> Get(Guid id)
    {
        var dto = await _service.GetByIdAsync(id);
        _broker.SendMessage(dto);
        return Ok(dto);
    }

    /// <summary>Create a new instance of <see cref="Sponsor"/>.</summary>
    /// <param name="dto"></param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a сreated <see cref="Sponsor"/> instance.
    /// </returns>
    /// <response code="201">Returns the created <see cref="Sponsor"/> instance.</response>
    /// <response code="400">If <see cref="Sponsor"/> instance creating is failed </response>
    /// <response code="422">If <see cref="Sponsor"/> instance is not valid.</response>
    /// <response code="500">If the generic error in the server.</response>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] SponsorDto dto)
    {
        var result = await _service.CreateAsync(dto);
        _broker.SendMessage(result);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>Modify an instance of <see cref="Sponsor"/> by <paramref name="id"/> identifier.</summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns>Modified <see cref="Sponsor"/> instance.</returns>
    /// <response code="202">Returns the modified <see cref="Sponsor"/> instance.</response>
    /// <response code="400">If <see cref="Sponsor"/> instance modifying is failed </response>
    /// <response code="422">If <see cref="Sponsor"/> instance is not valid.</response>
    /// <response code="500">If the generic error in the server.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] SponsorDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        _broker.SendMessage(result);
        return AcceptedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>Remove an instance of <see cref="Sponsor"/> by <paramref name="id"/> identifier.</summary>
    /// <param name="id"></param>
    /// <response code="204">IF <see cref="Sponsor"/> instance removing is successfull.</response>
    /// <response code="400">If <see cref="Sponsor"/> instance removing is failed </response>
    /// <response code="404">If <see cref="Sponsor"/> instance not found.</response>
    /// <response code="500">If the generic error in the server.</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
