using System.Text;
using dotenv.net;
using VehicleTracking.Application.Enums;
using VehicleTracking.Application.Exceptions.Environment;
using VehicleTracking.Application.Models;
using VehicleTracking.Domain.Enums;

namespace VehicleTracking.Application.Common;

// CONNECTION_STRING="Server=127.0.0.1;Port=5432;Database=myDataBase;User Id=myUsername;Password=myPassword;"
// CREDENTIALS_MAX_LENGTH=1024
// MISC_MAX_LENGTH=128
// STANDARD_ROLE="User"
// ASPNETCORE_ENVIRONMENT="Development"

public class EnvironmentUtilities
{
    private static bool _initialized;
    private static readonly Dictionary<string, EnvironmentDefaultValueDto> Default = new () {
        {"CONNECTION_STRING", new ("Server=127.0.0.1;Port=5432;Database=tracking;User Id=postgres;Password=postgres;", typeof(string))},
        {"CREDENTIALS_MAX_LENGTH", new ("1024", typeof(int))},
        {"MISC_MAX_LENGTH", new ("128", typeof(int))},
        {"STANDARD_ROLE", new ("User", typeof(UserRole))},
        {"ASPNETCORE_ENVIRONMENT", new("Development", typeof(DeploymentType))},
        {"ASPNETCORE_URLS", new("http://localhost:5001;https://localhost:7001", typeof(string))},
        {"LOGIN_PATH", new("/login", typeof(string))},
        {"UNAUTHORIZED_PATH", new("/unauthorized", typeof(string))},
    };
    
    private static object ConvertToObject(string key)
    {
        if (!Default.ContainsKey(key))
            throw new EnvironmentInvalidRequestedKey(key);

        var value = Environment.GetEnvironmentVariable(key);

        if (value == null)
            throw new EnvironmentValuesMissing(key);

        try
        {
            return TypeUtilities.Parse(value, Default[key].Type);
        }
        catch (Exception exception)
        {
            throw new EnvironmentFailedConverting(key, exception);
        }
    }
    
    public static T GetVariable<T>(string key)
    {
        // Ensure bootstrap on first access;
        Bootstrap();
        
        if (ConvertToObject(key) is T value)
        {
            return value;
        }

        throw new EnvironmentInvalidRequestedType(key);
    }
    
    public static void Bootstrap()
    {
        if (_initialized)
            return;
        
        _initialized = true;
        
        DotEnv.Load(options: new DotEnvOptions(
            ignoreExceptions: true,
            encoding: Encoding.UTF8,
            trimValues: true,
            overwriteExistingVars: true,
            probeForEnv: true,
            probeLevelsToSearch: 4,
            supportExportSyntax: true 
        ));
        
        foreach (var pair in Default)
        {
            var value = Environment.GetEnvironmentVariable(pair.Key);

            if (value == null)
                throw new EnvironmentValuesMissing(pair.Key);

            if (!TypeUtilities.Validate(value, pair.Value.Type))
                throw new EnvironmentInvalidType(pair.Key, pair.Value.Type);
        }

        // Run additional check for production
        if (GetVariable<DeploymentType>("ASPNETCORE_ENVIRONMENT") == DeploymentType.Production)
        {
            foreach (var pair in Default)
            {
                var value = Environment.GetEnvironmentVariable(pair.Key);
                
                if (pair.Key.EndsWith("PASSWORD") && value == pair.Value.Value)
                    throw new EnvironmentDefaultValues(pair.Key);
            }
        }
    }
    
}