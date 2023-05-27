namespace MeetupManager.Core.Repositories;

public class ApiRepository<T> : IApiRepository<T> where T : BaseModel
{
    private readonly ApiDbContext _apiDb;
    private readonly DbSet<T> _dbSet;

    public ApiRepository(ApiDbContext apiDb)
    {
        _apiDb = apiDb;
        _dbSet=_apiDb.Set<T>();
    }

    /// <summary>
    /// Get <see cref="BaseModel"/> collection by expression.
    /// </summary>
    /// <param name="expression">
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="List{T}"/> that contains elements from the db context by expression.
    /// </returns>
    public async Task<IEnumerable<T>> GetAllByAsync(Expression<Func<T, bool>>? expression = null)
    {
        IQueryable<T> query = _dbSet;
        query=(expression is null) ? query : query.Where(expression);
        var result= await query.AsNoTracking().ToListAsync();
        return result;
    }

    /// <summary>
    /// Get one <see cref="BaseModel"/> by expression.
    /// </summary>
    /// <param name="expression">
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="BaseModel"/> from the db context by expression.
    /// </returns>
    public async Task<T> GetOneByAsync(Expression<Func<T, bool>> expression)
    {
        IQueryable<T> query = _dbSet;
        var result=await query.FirstOrDefaultAsync(expression);
#pragma warning disable CS8603 // Possible null reference return.
        return result;
#pragma warning restore CS8603 // Possible null reference return.
    }

    /// <summary>
    /// Create instance of <see cref="BaseModel"/> on db context.
    /// </summary>
    /// <param name="model">
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a created instance of <see cref="BaseModel"/>.
    /// </returns>
    public async Task<T> CreateAsync(T model)
    {
        var result=_dbSet.Entry(model);
        result.State = EntityState.Added;
        await SaveDbChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Change existing instance of <see cref="BaseModel"/> on db context.
    /// </summary>
    /// <param name="model">
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a updated instance of <see cref="BaseModel"/>.
    /// </returns>
    public async Task<T> UpdateAsync(T model)
    {
        var result = _dbSet.Entry(model);
        result.State= EntityState.Modified;
        await SaveDbChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Remove instance of <see cref="BaseModel"/> from db context by expression.
    /// </summary>
    /// <param name="expression">
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="bool"/> result of <see cref="BaseModel"/> instance removing.
    /// </returns>
    public async Task<bool> DeleteByAsync(Expression<Func<T, bool>> expression)
    {
        var model = await _dbSet.FirstOrDefaultAsync(expression);

#pragma warning disable CS8604 // Possible null reference argument.
        var result = _dbSet.Remove(model) is not null;
#pragma warning restore CS8604 // Possible null reference argument.

        await SaveDbChangesAsync();
        return result;
    }

    /// <summary>
    /// Get information about <see cref="BaseModel"/> instanse existing on db context by expression. 
    /// </summary>
    /// <param name="expression">
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="bool"/> result of <see cref="BaseModel"/> instance existing.
    /// </returns>
    public async Task<bool> IsContains(Expression<Func<T, bool>>? expression = null)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        var result = await _dbSet.AnyAsync(expression);
#pragma warning restore CS8604 // Possible null reference argument.

        return result;
    }

    /// <summary>
    /// Get type of <see cref="BaseModel"/> instanse.
    /// </summary>
    /// <returns>
    /// <see cref="string"/> with name of <see cref="BaseModel"/> instance type. 
    /// </returns>
    public string GetModelType()
        => typeof(T).Name;


    /// <summary>
    /// Save db context changes.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="bool"/> result of db context changes saving.
    /// </returns>
    private async Task SaveDbChangesAsync()
    {
        var result = await _apiDb.SaveChangesAsync();
        if (result is 0) throw new InvalidSavingChangesException("Changes saving is failed");
    }
}
