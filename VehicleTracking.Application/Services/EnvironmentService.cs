using System.Text;
using dotenv.net;
using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Interfaces;

namespace VehicleTracking.Application.Services;

public class EnvironmentService : IEnvironmentService
{
    private List<KeyValuePair<string, string>> _default = [
        new ("ENVIRONMENT", "Development"),
        new ("POSTGRES_DB", "Server=127.0.0.1;Port=5432;Database=myDataBase;User Id=myUsername;Password=myPassword;")
    ];
    
    public EnvironmentService()
    {
        DotEnv.Load(options: new DotEnvOptions(
            ignoreExceptions: false,           // Throw on errors instead of silently failing (default: true)
            encoding: Encoding.UTF8,           // File encoding (default: UTF-8)
            trimValues: true,                  // Strip whitespace from values (default: false)
            overwriteExistingVars: true,       // Skip vars already set in the environment (default: true)
            probeForEnv: true,                 // Search parent directories for a .env file (default: false)
            probeLevelsToSearch: 4,            // How many directory levels to ascend when probing (default: 4)
            supportExportSyntax: true          // Support `export KEY=VALUE` syntax (default: false)
        ));

        
        foreach (var pair in _default)
        {
            var value = Environment.GetEnvironmentVariable(pair.Key);

            if (value == null)
                throw new EnvironmentValuesMissing();

            if (pair.Key.EndsWith("PASSWORD") && value == pair.Value)
                throw new EnvironmentDefaultValues();
        }
    }

    public string? GetVariable(string key)
    {
        return Environment.GetEnvironmentVariable(key);
    }
}