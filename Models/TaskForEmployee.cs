namespace project_management_system.Models;

using System.ComponentModel.DataAnnotations;

public class TaskForEmployee
{
    [Required]
    public required Guid TaskID { get; set; }
    [Required]
    [RegularExpression("^[0-9]+$", ErrorMessage = "the project ID contains invalid characters")]
    public required string ProjectID { get; set; }
    [Required]
    [RegularExpression("^[a-zA-Zа-яА-Я0-9]+$", ErrorMessage = "the title contains invalid characters")]
    public required string Title { get; set; }
    public required string Description { get; set; } = "";
    public required string? EmployeeLogin { get; set; }
    public TaskForEmployeeStatus Status { get; set; } = TaskForEmployeeStatus.ToDo;
}