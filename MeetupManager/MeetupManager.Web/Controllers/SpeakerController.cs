namespace MeetupManager.Web.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Authorize(AuthenticationSchemes = "Bearer")] //may be without schema, it sets by di
public class SpeakerController : ControllerBase
{
    private readonly IApiService<SpeakerDto> _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public SpeakerController(IApiService<SpeakerDto> service)
    {
        _service= service;
    }

    /// <summary>
    /// Get all of <see cref="Speaker"/> collection.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Speaker"/> collection.
    /// </returns>
    /// <response code="200">Returns the <see cref="Speaker"/> collection</response>
    /// <response code="204">If <see cref="Speaker"/> collection is empty.</response>
    /// <response code="404">If <see cref="Speaker"/> collection not found.</response>
    /// <response code="500">If the generic error in the server.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SpeakerDto>>> Get()
    {
        var dtos = await _service.GetAsync();
        return Ok(dtos);
    }

    /// <summary>Get <see cref="Speaker"/> by <paramref name="id"/> identifier.</summary>
    /// <param name="id"></param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Speaker"/> instance.
    /// </returns>
    /// <response code="200">Returns the <see cref="Speaker"/> collection</response>
    /// <response code="404">If <see cref="Speaker"/> instance not found.</response>
    /// <response code="500">If the generic error in the server.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<SpeakerDto>> Get(Guid id)
    {
        var dto = await _service.GetByIdAsync(id);
        return Ok(dto);
    }

    /// <summary>Create a new instance of <see cref="Speaker"/>.</summary>
    /// <param name="dto"></param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a сreated <see cref="Speaker"/> instance.
    /// </returns>
    /// <response code="201">Returns the created <see cref="Speaker"/> instance.</response>
    /// <response code="400">If <see cref="Speaker"/> instance creating is failed </response>
    /// <response code="422">If <see cref="Speaker"/> instance is not valid.</response>
    /// <response code="500">If the generic error in the server.</response>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] SpeakerDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>Modify an instance of <see cref="Speaker"/> by <paramref name="id"/> identifier.</summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns>Modified <see cref="Speaker"/> instance.</returns>
    /// <response code="202">Returns the modified <see cref="Speaker"/> instance.</response>
    /// <response code="400">If <see cref="Speaker"/> instance modifying is failed </response>
    /// <response code="422">If <see cref="Speaker"/> instance is not valid.</response>
    /// <response code="500">If the generic error in the server.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] SpeakerDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return AcceptedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>Remove an instance of <see cref="Speaker"/> by <paramref name="id"/> identifier.</summary>
    /// <param name="id"></param>
    /// <response code="204">IF <see cref="Speaker"/> instance removing is successfull.</response>
    /// <response code="400">If <see cref="Speaker"/> instance removing is failed </response>
    /// <response code="404">If <see cref="Speaker"/> instance not found.</response>
    /// <response code="500">If the generic error in the server.</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
