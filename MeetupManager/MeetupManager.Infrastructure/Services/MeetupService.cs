namespace MeetupManager.Infrastructure.Services;

public sealed class MeetupService:ApiService<Meetup,MeetupDto>
{
    private readonly IApiRepository<Meetup> _meetupRep;
    private readonly IApiRepository<Speaker> _speakerRep;
    private readonly IApiRepository<Sponsor> _sponsorpRep;


    private readonly IMapper _mapper;
    private readonly ILogger<MeetupService> _logger;
    private readonly IValidator<MeetupDto> _validator;

    private readonly string _modelType;

    public MeetupService(
        IApiRepository<Meetup> meetuRep,
        IApiRepository<Speaker> speakerRep,
        IApiRepository<Sponsor> sponsorpRep,
        IMapper mapper,
        ILogger<MeetupService> logger,
        IValidator<MeetupDto> validator)
        : base(meetuRep, mapper, logger, validator)
    {
        _meetupRep= meetuRep;
        _speakerRep= speakerRep;
        _sponsorpRep= sponsorpRep;
        _mapper= mapper;
        _logger=logger;
        _validator= validator;
        _modelType=_meetupRep.GetModelType();
    }

    public override async Task<IEnumerable<MeetupDto>> GetAsync()
    {
        var models = await _meetupRep.GetAllByAsync()
            ?? throw new ContentNotFoundException($"'{_modelType}' collection not found");

        _logger.LogInformation("'{ModelType}' collection loaded successfully", _modelType);
        var dtos = _mapper.Map<IEnumerable<MeetupDto>>(models).OrderBy(_ => _.Spending);

        return dtos;
    }

    public override async Task<MeetupDto> CreateAsync(MeetupDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            throw new ModelNotValidException($"Invalid value for '{_modelType}'");        

        foreach(var speakerId in dto.SpeakerIds)
        {
            var speaker = await _speakerRep.GetOneByAsync(_=>_.Id.Equals(speakerId));
            if (speaker is not null) dto.Speakers.Add(speaker);
        }

        foreach (var speakerId in dto.SponsorIds)
        {
            var speaker = await _speakerRep.GetOneByAsync(_ => _.Id.Equals(speakerId));
            if (speaker is not null) dto.Speakers.Add(speaker);
        }

        var model = _mapper.Map<Meetup>(dto);

        var modelResult = await _meetupRep.CreateAsync(model)
            ?? throw new InvalidOperationException($"'{_modelType}' creating failed");

        _logger.LogInformation("'{ModelType}' with Id '{ModelId}' created successfully", _modelType, modelResult!.Id);
        var dtoResult = _mapper.Map<MeetupDto>(modelResult);
        return dtoResult;
    }

    public override async Task<MeetupDto> UpdateAsync(Guid id, MeetupDto dto)
    {
        if (!dto.Id.Equals(id))
            throw new InvalidOperationException($"'{_modelType}' Id '{dto.Id}' does not match Id '{id}' from request");

        if (!await _meetupRep.IsContains(_ => _.Id.Equals(id)))
            throw new ContentNotFoundException($"'{_modelType}' with Id '{id}' not found");

        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            throw new ModelNotValidException($"Invalid value for '{_modelType}'");

        foreach (var speakerId in dto.SpeakerIds)
        {
            var speaker = await _speakerRep.GetOneByAsync(_ => _.Id.Equals(speakerId));
            if (speaker is not null) dto.Speakers.Add(speaker);
        }

        foreach (var speakerId in dto.SponsorIds)
        {
            var speaker = await _speakerRep.GetOneByAsync(_ => _.Id.Equals(speakerId));
            if (speaker is not null) dto.Speakers.Add(speaker);
        }

        var model = _mapper.Map<Meetup>(dto);

        var modelResult = await _meetupRep.UpdateAsync(model)
            ?? throw new InvalidOperationException($"'{_modelType}' updating failed");

        var dtoResult = _mapper.Map<MeetupDto>(modelResult);
        _logger.LogInformation("'{ModelType}' modified successfully", _modelType);
        return dtoResult;
    }
}
