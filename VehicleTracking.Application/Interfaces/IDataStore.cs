using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Application.Interfaces;

public interface IDataStore
{
    Task<IReadOnlyCollection<User>> GetUsersAsync();
    Task<User> AddUserAsync(User user);
}