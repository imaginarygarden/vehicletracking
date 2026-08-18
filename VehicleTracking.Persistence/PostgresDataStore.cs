using Microsoft.EntityFrameworkCore;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Domain.Interfaces;

namespace VehicleTracking.Persistence;

public class PostgresDataStore(IDbContextFactory<VehicleTrackingDbContext> factory) : IDataStore
{
    public async Task<TResult> QueryAsync<TSource, TResult>(Func<IQueryable<TSource>, Task<TResult>> query) where TSource : class, IEntity
    {
        await using var context = await factory.CreateDbContextAsync();

        return await query(context.Set<TSource>().AsNoTracking());
    }

    public async Task<T?> AddAsync<T>(T entity) where T : class, IEntity
    {
        await using var context = await factory.CreateDbContextAsync();
        await context.AddAsync(entity);
        var count = await context.SaveChangesAsync();

        if (count < 1)
            return null;

        return entity;
    }
    
    public async Task<T?> UpdateAsync<T>(T entity) where T : class, IEntity
    {
        await using var context = await factory.CreateDbContextAsync();
        context.Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        var count = await context.SaveChangesAsync();

        if (count < 1)
            return null;

        return entity;
    }
    
    public async Task<T?> RemoveAsync<T>(T entity) where T : class, IEntity
    {
        await using var context = await factory.CreateDbContextAsync();
        context.Remove(entity);
        var count = await context.SaveChangesAsync();

        if (count < 1)
            return null;

        return entity;
    }
}