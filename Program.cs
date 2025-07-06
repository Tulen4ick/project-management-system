using project_management_system;
using Microsoft.Extensions.Configuration;

public class Program
{
    static void Main()
    {
        string logPath = "";
        string dbPath = "";
        try
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            logPath = config["Logging:LogPath"] ?? throw new Exception("Configuration log key not found");
            dbPath = config["DB:DBPath"] ?? throw new Exception("Configuration db key not found");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Configuration error: {ex.Message}");
        }
        if (string.IsNullOrWhiteSpace(logPath) || !File.Exists(logPath))
            logPath = "./Logs/logs.txt";
        if (string.IsNullOrWhiteSpace(dbPath) || !File.Exists(dbPath))
            dbPath = "./Data/db.json";
        ConsoleUI.RunApp(logPath, dbPath);
    }
}
