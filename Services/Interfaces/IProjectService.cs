using project_management_system.Models;

namespace project_management_system.Services.Interfaces;

public interface IProjectService
{
    void CreateTask(TaskForEmployee task);
    void AssignEmployeeToTask(Guid taskId, string employeeLogin);
    List<TaskForEmployee> GetEmployeeTasks(string login);
    void UpdateTaskStatus(Guid taskId, TaskForEmployeeStatus status);
    List<TaskForEmployee> GetAllTasks();
}