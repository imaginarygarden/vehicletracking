using System.Text;
using dotenv.net;
using VehicleTracking.Application.Enums;
using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Models;

namespace VehicleTracking.Application.Services;

public class EnvironmentService : IEnvironmentService
{
    private readonly Dictionary<string, EnvironmentDefaultValueDto> _default = new () {
        {"ASPNETCORE_ENVIRONMENT", new("Development", typeof(DeploymentType))},
        {"POSTGRES_DB", new ("Server=127.0.0.1;Port=5432;Database=myDataBase;User Id=myUsername;Password=myPassword;", typeof(string))}
    };

    private IUtilityService _utilityService;
    
    private object ConvertToObject(string key)
    {
        if (!_default.ContainsKey(key))
            throw new EnvironmentInvalidRequestedKey(key);

        var value = Environment.GetEnvironmentVariable(key);

        if (value == null)
            throw new EnvironmentValuesMissing(key);

        try
        {
            return _utilityService.ConvertToObject(value, _default[key].Type);
        }
        catch (Exception exception)
        {
            throw new EnvironmentFailedConverting(key, exception);
        }
    }
    
    public EnvironmentService(IUtilityService utilityService)
    {
        _utilityService = utilityService;

        DotEnv.Load(options: new DotEnvOptions(
            ignoreExceptions: true,
            encoding: Encoding.UTF8,
            trimValues: true,
            overwriteExistingVars: true,
            probeForEnv: true,
            probeLevelsToSearch: 4,
            supportExportSyntax: true 
        ));
        
        foreach (var pair in _default)
        {
            var value = Environment.GetEnvironmentVariable(pair.Key);

            if (value == null)
                throw new EnvironmentValuesMissing(pair.Key);

            if (!utilityService.IsValidType(value, pair.Value.Type))
                throw new EnvironmentInvalidType(pair.Key, pair.Value.Type);
        }

        // Run additional check for production
        if (GetVariable<DeploymentType>("ASPNETCORE_ENVIRONMENT") == DeploymentType.Production)
        {
            foreach (var pair in _default)
            {
                var value = Environment.GetEnvironmentVariable(pair.Key);
                
                if (pair.Key.EndsWith("PASSWORD") && value == pair.Value.Value)
                    throw new EnvironmentDefaultValues(pair.Key);
            }
        }
    }

    public T GetVariable<T>(string key)
    {
        if (ConvertToObject(key) is T value)
        {
            return value;
        }

        throw new EnvironmentInvalidRequestedType(key);
    }
}