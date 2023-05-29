namespace MeetupManager.Core.Interfaces;

public interface IApiRepository<T> where T : BaseModel
{
    public Task<IEnumerable<T>> GetAllByAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool? isTracking = null);
    public Task<T> GetOneByAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        bool? isTracking = null);
    public Task<T> CreateAsync(T model);
    public Task<T> UpdateAsync(T model);
    public Task<bool> DeleteByAsync(Expression<Func<T, bool>> expression);
    public Task<bool> IsContains(Expression<Func<T, bool>>? expression = null);
    public string GetModelType();
    public void Attach(BaseModel model);
    public void SetValues(T objectModel, T subjectModel);
}
