using VehicleTracking.Domain.Interfaces;

namespace VehicleTracking.Application.Interfaces;

public interface IDataStore
{
    Task<TResult> QueryAsync<TSource, TResult>(Func<IQueryable<TSource>, Task<TResult>> query)
        where TSource : class, IEntity;
    Task<T?> AddAsync<T>(T entity) where T : class, IEntity;
    Task<T?> UpdateAsync<T>(T entity) where T : class, IEntity;
    Task<T?> RemoveAsync<T>(T entity) where T : class, IEntity;
}