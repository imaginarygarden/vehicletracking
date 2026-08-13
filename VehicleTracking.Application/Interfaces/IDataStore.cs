using System.Linq.Expressions;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Application.Interfaces;

public interface IDataStore
{
    Task<TResult> QueryAsync<TSource, TResult>(Func<IQueryable<TSource>, Task<TResult>> query)
        where TSource : class, IDbSetEntity;
    Task<T?> AddAsync<T>(T entity) where T : class, IDbSetEntity;
    Task<T?> RemoveAsync<T>(T entity) where T : class, IDbSetEntity;
}