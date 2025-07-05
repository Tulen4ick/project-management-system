using project_management_system;
using Microsoft.Extensions.Configuration;

public class Program
{
    static void Main()
    {
        var config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddEnvironmentVariables()
           .Build();
        string logPath = "";
        string dbPath = "";
        try
        {
            logPath = config["Logging:LogPath"] ?? throw new Exception("Configuration log key not found");
            dbPath = config["DB:DBPath"] ?? throw new Exception("Configuration db key not found");
            if (string.IsNullOrWhiteSpace(logPath))
                logPath = "./Logs/logs.txt";
            if (string.IsNullOrWhiteSpace(dbPath))
                dbPath = "./Data/db.json";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Configuration error: {ex.Message}");
        }
        ConsoleUI.RunApp(logPath, dbPath);
    }
}
