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
        var models = await _meetupRep.GetAllByAsync(
            include:_=>_.Include(_=>_.Speakers).Include(_=>_.Sponsors),
            orderBy:_=>_.OrderBy(_=>_.Spending))
            ?? throw new ContentNotFoundException($"'{_modelType}' collection not found");

        _logger.LogInformation("'{ModelType}' collection loaded successfully", _modelType);
        var dtos = new List<MeetupDto>();

        foreach (var model in models)
        {
            var dto = MapWithIdCollections(model);
            dtos.Add(dto);
        }

        return dtos;
    }

    public override async Task<MeetupDto> GetByIdAsync(Guid id)
    {
        if (!await _meetupRep.IsContains(_ => _.Id.Equals(id)))
            throw new ContentNotFoundException($"'{_modelType}' with Id '{id}' not found");

        var model = await _meetupRep.GetOneByAsync(
            expression: _ => _.Id.Equals(id),
            include: _ => _.Include(_ => _.Speakers).Include(_ => _.Sponsors));

        _logger.LogInformation("'{ModelType}' with Id '{ModelId}' loaded successfully", _modelType, id);
        var dto = MapWithIdCollections(model);
        return dto;
    }

    public override async Task<MeetupDto> CreateAsync(MeetupDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            throw new ModelNotValidException($"Invalid value for '{_modelType}'");

        var model = _mapper.Map<Meetup>(dto);

        foreach (var speakerId in dto.SpeakerIds)
        {
            var speakerFromDb = await _speakerRep.GetOneByAsync(_=>_.Id.Equals(speakerId),isTracking:true);
            if (speakerFromDb is not null) model.Speakers.Add(speakerFromDb);
        }

        foreach (var sponsorId in dto.SponsorIds)
        {
            var sponsorFromDb = await _sponsorpRep.GetOneByAsync(_ => _.Id.Equals(sponsorId), isTracking: true);
            if (sponsorFromDb is not null) model.Sponsors.Add(sponsorFromDb);
        }

        var modelResult = await _meetupRep.CreateAsync(model)
            ?? throw new InvalidOperationException($"'{_modelType}' creating failed");

        _logger.LogInformation("'{ModelType}' with Id '{ModelId}' created successfully", _modelType, modelResult!.Id);
        var dtoResult = MapWithIdCollections(modelResult);
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

        var meetupFromDb = await _meetupRep.GetOneByAsync(
                expression: _ => _.Id.Equals(id),
                include: _ => _.Include(_ => _.Speakers).Include(_ => _.Sponsors),
                isTracking:true);

        var subjectModel = _mapper.Map<Meetup>(dto);

        _meetupRep.SetValues(meetupFromDb, subjectModel);

        meetupFromDb.Speakers.Clear();
        meetupFromDb.Sponsors.Clear();

        foreach (var speakerId in dto.SpeakerIds.ToList())
        {
            var speakerFromDb = await _speakerRep.GetOneByAsync(
                expression: _=>_.Id.Equals(speakerId),
                isTracking: true);

            if (speakerFromDb is not null) meetupFromDb.Speakers.Add(speakerFromDb);
            else _meetupRep.Attach(speakerFromDb!);
        }

        foreach (var sponsorId in dto.SponsorIds.ToList())
        {
            var sponsorFromDb = await _sponsorpRep.GetOneByAsync(
                expression: _ => _.Id.Equals(sponsorId),
                isTracking: true);

            if (sponsorFromDb is not null) meetupFromDb.Sponsors.Add(sponsorFromDb);
            else _meetupRep.Attach(sponsorFromDb!);
        }

        var result=await _meetupRep.UpdateAsync(meetupFromDb)
            ?? throw new InvalidOperationException($"'{_modelType}' updating failed");

        var dtoResult=MapWithIdCollections(result);
        return dtoResult;
    }

    private MeetupDto MapWithIdCollections(Meetup model)
    {
        var dto = _mapper.Map<MeetupDto>(model);
        dto.SpeakerIds=model.Speakers.Select(s => s.Id).ToList();
        dto.SponsorIds=model.Sponsors.Select(s => s.Id).ToList();
        return dto;
    }
}
