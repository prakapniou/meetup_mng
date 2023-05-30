namespace MeetupManager.Web.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Authorize(AuthenticationSchemes ="Bearer")] //may be without schema, it sets by di
public class MeetupController:ControllerBase
{
    private readonly IApiService<MeetupDto> _service;
    private readonly IRabbitmqProducer _rabbitmqProducer;

   /// <summary>
   /// 
   /// </summary>
   /// <param name="service"></param>
   /// <param name="rabbitmqProducer"></param>
    public MeetupController(
        IApiService<MeetupDto> service,
        IRabbitmqProducer rabbitmqProducer)
    {
        _service= service;
        _rabbitmqProducer= rabbitmqProducer;
    }

    /// <summary>
    /// Get all of <see cref="Meetup"/> collection.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Meetup"/> collection.
    /// </returns>
    /// <response code="200">Returns the <see cref="Meetup"/> collection</response>
    /// <response code="204">If <see cref="Meetup"/> collection is empty.</response>
    /// <response code="404">If <see cref="Meetup"/> collection not found.</response>
    /// <response code="500">If the generic error in the server.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MeetupDto>>> Get()
    {
        var dtos = await _service.GetAsync();
        _rabbitmqProducer.SendMeetupMessage(dtos);
        return Ok(dtos);
    }

    /// <summary>Get <see cref="Meetup"/> by <paramref name="id"/> identifier.</summary>
    /// <param name="id"></param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Meetup"/> instance.
    /// </returns>
    /// <response code="200">Returns the <see cref="Meetup"/> collection</response>
    /// <response code="404">If <see cref="Meetup"/> instance not found.</response>
    /// <response code="500">If the generic error in the server.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<MeetupDto>> Get(Guid id)
    {
        var dto = await _service.GetByIdAsync(id);
        _rabbitmqProducer.SendMeetupMessage(dto);
        return Ok(dto);
    }

    /// <summary>Create a new instance of <see cref="Meetup"/>.</summary>
    /// <param name="dto"></param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a сreated <see cref="Meetup"/> instance.
    /// </returns>
    /// <response code="201">Returns the created <see cref="Meetup"/> instance.</response>
    /// <response code="400">If <see cref="Meetup"/> instance creating is failed </response>
    /// <response code="422">If <see cref="Meetup"/> instance is not valid.</response>
    /// <response code="500">If the generic error in the server.</response>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] MeetupDto dto)
    {
        var result = await _service.CreateAsync(dto);
        _rabbitmqProducer.SendMeetupMessage(result);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>Modify an instance of <see cref="Meetup"/> by <paramref name="id"/> identifier.</summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns>Modified <see cref="Meetup"/> instance.</returns>
    /// <response code="202">Returns the modified <see cref="Meetup"/> instance.</response>
    /// <response code="400">If <see cref="Meetup"/> instance modifying is failed </response>
    /// <response code="422">If <see cref="Meetup"/> instance is not valid.</response>
    /// <response code="500">If the generic error in the server.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] MeetupDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        _rabbitmqProducer.SendMeetupMessage(result);
        return AcceptedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>Remove an instance of <see cref="Meetup"/> by <paramref name="id"/> identifier.</summary>
    /// <param name="id"></param>
    /// <response code="204">IF <see cref="Meetup"/> instance removing is successfull.</response>
    /// <response code="400">If <see cref="Meetup"/> instance removing is failed </response>
    /// <response code="404">If <see cref="Meetup"/> instance not found.</response>
    /// <response code="500">If the generic error in the server.</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
