using project_management_system.Models;
using project_management_system.Services.Interfaces;

namespace project_management_system.Services.Implementations;

public class ProjectService : IProjectService
{
    private readonly IDataStorage _storage;
    private readonly IUserService _userService;
    public ProjectService(IDataStorage storage, IUserService userService)
    {
        _storage = storage;
        _userService = userService;
    }

    public void CreateTask(TaskForEmployee task)
    {
        var data = _storage.LoadData();
        data.Tasks.Add(task);
        _storage.SaveData(data);
    }

    public void AssignEmployeeToTask(Guid taskId, string employeeLogin)
    {
        var data = _storage.LoadData();
        var task = data.Tasks.FirstOrDefault(task => task.TaskID == taskId) ?? throw new KeyNotFoundException($"Task with {taskId} id does not exist");
        var user = _userService.FindUserByLogin(employeeLogin);
        if (user == null || user.Role != UserRoles.Employee)
        {
            throw new Exception($"The employee {employeeLogin} was not found or is not a regular employee");
        }
        task.EmployeeLogin = employeeLogin;
        _storage.SaveData(data);
    }

    public List<TaskForEmployee> GetEmployeeTasks(string login)
    {
        var data = _storage.LoadData();
        return data.Tasks.Where(task => task.EmployeeLogin == login).ToList();
    }

    public void UpdateTaskStatus(Guid taskId, TaskForEmployeeStatus status)
    {
        var data = _storage.LoadData();
        var task = data.Tasks.FirstOrDefault(task => task.TaskID == taskId) ?? throw new KeyNotFoundException($"Task with {taskId} id does not exist");
        task.Status = status;
        _storage.SaveData(data);
    }

    public List<TaskForEmployee> GetAllTasks()
    {
        return _storage.LoadData().Tasks;
    }

    public List<TaskForEmployee> FilterAndSortTasks(List<TaskForEmployee> tasks, TaskFilter options)
    {
        var filtered = tasks.AsQueryable();

        if (!string.IsNullOrEmpty(options.ProjectID))
            filtered = filtered.Where(t => t.ProjectID == options.ProjectID);

        if (options.Status != null)
            filtered = filtered.Where(t => t.Status == options.Status);

        if (!string.IsNullOrEmpty(options.AssignedEmployeeLogin))
            filtered = filtered.Where(t => t.EmployeeLogin == options.AssignedEmployeeLogin);

        filtered = options.SortBy?.ToLower() switch
        {
            "id" => options.SortDescending ?
                filtered.OrderByDescending(t => t.TaskID) :
                filtered.OrderBy(t => t.TaskID),

            "title" => options.SortDescending ?
                filtered.OrderByDescending(t => t.Title) :
                filtered.OrderBy(t => t.Title),

            "project" => options.SortDescending ?
                filtered.OrderByDescending(t => t.ProjectID) :
                filtered.OrderBy(t => t.ProjectID),

            "status" => options.SortDescending ?
                filtered.OrderByDescending(t => t.Status) :
                filtered.OrderBy(t => t.Status),

            "employee" => options.SortDescending ?
                filtered.OrderByDescending(t => t.EmployeeLogin) :
                filtered.OrderBy(t => t.EmployeeLogin),

            _ => options.SortDescending ?
                filtered.OrderByDescending(t => t.TaskID) :
                filtered.OrderBy(t => t.TaskID)
        };

        return filtered.ToList();
    }
}