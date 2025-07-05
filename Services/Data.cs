namespace project_management_system.Services;

using project_management_system.Models;

public class Data
{
    public List<User> Users { get; set; } = new List<User>();
    public List<TaskForEmployee> Tasks { get; set; } = new List<TaskForEmployee>();
}