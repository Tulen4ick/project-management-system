namespace project_management_system.Models;

using System.ComponentModel.DataAnnotations;

public class TaskForEmployee
{
    [Required]
    public required Guid TaskID { get; set; }
    [Required]
    public required string ProjectID { get; set; }
    [Required]
    public required string Title { get; set; }
    public required string Description { get; set; } = "";
    [Required]
    public required string? EmployeeLogin { get; set; }
    public TaskForEmployeeStatus Status { get; set; } = TaskForEmployeeStatus.ToDo;
}