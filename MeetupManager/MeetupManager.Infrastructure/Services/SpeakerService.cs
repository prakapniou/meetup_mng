﻿namespace MeetupManager.Infrastructure.Services;

public sealed class SpeakerService:ApiService<Speaker,SpeakerDto>
{
    private readonly IApiRepository<Speaker> _rep;
    private readonly IMapper _mapper;
    private readonly ILogger<SpeakerService> _logger;
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
        _modelType=_rep.GetModelType();
    }

    public override async Task<IEnumerable<SpeakerDto>> GetAsync()
    {
        var models = await _rep.GetAllByAsync()
            ?? throw new ContentNotFoundException($"'{_modelType}' collection not found");

        _logger.LogInformation("'{ModelType}' collection loaded successfully", _modelType);
        var dtos = _mapper.Map<IEnumerable<SpeakerDto>>(models).OrderBy(_ => _.Name);
        return dtos;
    }
}
