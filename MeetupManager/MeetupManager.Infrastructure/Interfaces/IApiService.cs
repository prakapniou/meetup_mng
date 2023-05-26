namespace MeetupManager.Infrastructure.Interfaces;

public interface IApiService<Tdto> where Tdto : BaseDto
{
    public Task<IEnumerable<Tdto>> GetAsync();
    public Task<Tdto> GetByIdAsync(Guid id);
    public Task<Tdto> CreateAsync(Tdto dto);
    public Task<Tdto> UpdateAsync(Guid id, Tdto dto);
    public Task<bool> DeleteAsync(Guid id);
}
