using System.Reflection.Metadata;
using System.Text.Json;
using project_management_system.Models;
using project_management_system.Services;
using project_management_system.Services.Interfaces;

public class JsonStorage : IDataStorage
{
    private readonly string FilePath;

    public JsonStorage(string filePath)
    {
        FilePath = filePath;
    }

    public Data LoadData()
    {
        if (!File.Exists(FilePath))
        {
            File.WriteAllText(FilePath, "[]");
            return new Data();
        }
        string json = File.ReadAllText(FilePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            json = "[]";
        }
        try
        {
            return JsonSerializer.Deserialize<Data>(json) ?? new Data();
        }
        catch (JsonException)
        {
            File.WriteAllText(FilePath, "[]");
            return new Data();
        }
    }

    public void SaveData(Data data)
    {
        var json = JsonSerializer.Serialize(data);
        File.WriteAllText(FilePath, json);
    }
}