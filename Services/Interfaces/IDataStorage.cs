namespace project_management_system.Services.Interfaces;
public interface IDataStorage
{
    Data LoadData();
    void SaveData(Data data);
}