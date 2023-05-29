namespace MeetupManager.Infrastructure.Services;

public sealed class SpeakerService:ApiService<Speaker,SpeakerDto>
{
    private readonly IApiRepository<Speaker> _rep;
    private readonly IMapper _mapper;
    private readonly ILogger<SpeakerService> _logger;
    private readonly IValidator<SpeakerDto> _validator;
    private readonly string _modelType;

    public SpeakerService(
        IApiRepository<Speaker> rep,
        IMapper mapper,
        ILogger<SpeakerService> logger,
        IValidator<SpeakerDto> validator)
        : base(rep, mapper, logger, validator)
    {
        _rep= rep;
        _mapper= mapper;
        _logger=logger;
        _validator= validator;
        _modelType=_rep.GetModelType();
    }

    public override async Task<IEnumerable<SpeakerDto>> GetAsync()
    {
        var models = await _rep.GetAllByAsync(orderBy: _ => _.OrderBy(_=>_.Name))
            ?? throw new ContentNotFoundException($"'{_modelType}' collection not found");

        _logger.LogInformation("'{ModelType}' collection loaded successfully", _modelType);
        var dtos = _mapper.Map<IEnumerable<SpeakerDto>>(models);
        return dtos;
    }
}
