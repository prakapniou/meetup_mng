namespace MeetupManager.Infrastructure.Services;

public abstract class ApiService<Tmodel,Tdto>:IApiService<Tdto>
    where Tmodel : BaseModel
    where Tdto : BaseDto
{
    private readonly IApiRepository<Tmodel> _rep;
    private readonly IMapper _mapper;
    private readonly ILogger<ApiService<Tmodel, Tdto>> _logger;
    private readonly IValidator<Tdto> _validator;
    private readonly string _modelType;

    public ApiService(
        IApiRepository<Tmodel> rep,
        IMapper mapper,
        ILogger<ApiService<Tmodel, Tdto>> logger,
        IValidator<Tdto> validator)
    {
        _rep = rep;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
        _modelType=_rep.GetModelType();
    }

    public virtual async Task<IEnumerable<Tdto>> GetAsync()
    {
        if (!await _rep.IsContains()) throw new ContentNotFoundException($"'{_modelType}' collection not found");

        var models = await _rep.GetAllByAsync();
        _logger.LogInformation("'{ModelType}' collection loaded successfully", _modelType);
        var dtos = _mapper.Map<IEnumerable<Tdto>>(models);
        return dtos;
    }

    public virtual async Task<Tdto> GetByIdAsync(Guid id)
    {
        if (!await _rep.IsContains(_ => _.Id.Equals(id))) 
            throw new ContentNotFoundException($"'{_modelType}' with Id '{id}' not found");

        var model = await _rep.GetOneByAsync(_ => _.Id.Equals(id));
        _logger.LogInformation("'{ModelType}' with Id '{ModelId}' loaded successfully", _modelType, id);
        var dto = _mapper.Map<Tdto>(model);
        return dto;
    }

    public async Task<Tdto> CreateAsync(Tdto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            throw new ModelNotValidException($"Invalid value for '{_modelType}'");

        var model = _mapper.Map<Tmodel>(dto);
        var modelResult = await _rep.CreateAsync(model)
            ?? throw new InvalidOperationException($"'{_modelType}' creating failed");

        _logger.LogInformation("'{ModelType}' with Id '{ModelId}' created successfully", _modelType, modelResult!.Id);
        var dtoResult = _mapper.Map<Tdto>(modelResult);
        return dtoResult;
    }

    public async Task<Tdto> UpdateAsync(Guid id, Tdto dto)
    {
        if (!dto.Id.Equals(id))
            throw new InvalidOperationException($"'{_modelType}' Id '{dto.Id}' does not match Id '{id}' from request");

        if (!await _rep.IsContains(_ => _.Id.Equals(id)))
            throw new ContentNotFoundException($"'{_modelType}' with Id '{id}' not found");

        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            throw new ModelNotValidException($"Invalid value for '{_modelType}'");

        var model = _mapper.Map<Tmodel>(dto);
        var modelResult = await _rep.UpdateAsync(model)
            ?? throw new InvalidOperationException($"'{_modelType}' updating failed");

        var dtoResult = _mapper.Map<Tdto>(modelResult);
        _logger.LogInformation("'{ModelType}' modified successfully", _modelType);
        return dtoResult;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        Expression<Func<Tmodel, bool>> expression = _ => _.Id.Equals(id);

        if (!await _rep.IsContains(expression))
            throw new ContentNotFoundException
                ($"'{_modelType}' with Id '{id}' not found");

        var result = await _rep.DeleteByAsync(expression);

        if (!result) throw new InvalidOperationException($"'{_modelType}' with Id '{id}' removing failed");

        _logger.LogInformation("'{ModelType}' with Id '{ModelId}' removed successfully", _modelType, id);
        return result;
    }
}
