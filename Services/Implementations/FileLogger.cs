using project_management_system.Services.Interfaces;

namespace project_management_system.Services.Implementations;

public class FileLogger : ILog
{
    private readonly string _filePath;

    public FileLogger(string filepath)
    {
        if (filepath != null)
        {
            _filePath = filepath;
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
        }
    }

    public void Log(string message)
    {
        File.AppendAllText(_filePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}\n");
    }
}