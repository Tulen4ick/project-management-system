namespace project_management_system.Models;

public class TaskFilter
{
    public string? ProjectID { get; set; }
    public TaskForEmployeeStatus? Status { get; set; }
    public string? AssignedEmployeeLogin { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; }
}