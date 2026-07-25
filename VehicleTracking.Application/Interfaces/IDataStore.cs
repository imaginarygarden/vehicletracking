using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Application.Interfaces;

public interface IDataStore
{
    Task<IReadOnlyCollection<T>> GetAsync<T>() where T : class, IDbSetEntity;
    Task<T?> AddAsync<T>(T entity) where T : class, IDbSetEntity;
}