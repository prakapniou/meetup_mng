namespace MeetupManager.Infrastructure.Services;

public sealed class SponsorService:ApiService<Sponsor,SponsorDto>
{
    private readonly IApiRepository<Sponsor> _rep;
    private readonly IMapper _mapper;
    private readonly ILogger<SponsorService> _logger;
    private readonly IValidator<SponsorDto> _validator;
    private readonly string _modelType;

    public SponsorService(
        IApiRepository<Sponsor> rep,
        IMapper mapper,
        ILogger<SponsorService> logger,
        IValidator<SponsorDto> validator)
        : base(rep, mapper, logger, validator)
    {
        _rep= rep;
        _mapper= mapper;
        _logger=logger;
        _validator= validator;
        _modelType=_rep.GetModelType();
    }

    public override async Task<IEnumerable<SponsorDto>> GetAsync()
    {
        var models = await _rep.GetAllByAsync()
            ?? throw new ContentNotFoundException($"'{_modelType}' collection not found");

        _logger.LogInformation("'{ModelType}' collection loaded successfully", _modelType);
        var dtos = _mapper.Map<IEnumerable<SponsorDto>>(models).OrderBy(_ => _.Name);
        return dtos;
    }
}
