using Microsoft.EntityFrameworkCore;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Persistence;

public class PostgresDataStore(IDbContextFactory<VehicleTrackingDbContext> factory) : IDataStore
{
    async Task<IReadOnlyCollection<T>> GetAsync<T>() where T : class, IDbSetEntity
    {
        await using var context = await factory.CreateDbContextAsync();
        
        var set = context.Set<T>();
        var query = await set.AsNoTracking().ToListAsync();
        
        return query;
    }

    async Task<T> AddAsync<T>(T entity) where T : class, IDbSetEntity
    {
        await using var context = await factory.CreateDbContextAsync();
        await context.AddAsync(entity);
        var count = await context.SaveChangesAsync();

        if (count < 1)
            throw new Exception(); // implement correct exception

        context.Update(entity);
        return entity;
    }
}