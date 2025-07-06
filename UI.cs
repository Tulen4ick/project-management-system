using System.Reflection;
using project_management_system.Models;
using project_management_system.Services.Implementations;
using project_management_system.Services.Interfaces;
namespace project_management_system;

public static class ConsoleUI
{

    private static IDataStorage _storage;
    private static IAuthService _authService;
    private static IUserService _userService;
    private static IProjectService _projectService;
    private static IPasswordService _passwordService;
    private static ILog _log;

    private static void InitializeServices(string logPath, string dbPath)
    {
        _storage = new JsonStorage(dbPath);
        _passwordService = new PasswordService();
        _authService = new AuthService(_storage, _passwordService);
        _userService = new UserService(_storage, _passwordService);
        _projectService = new ProjectService(_storage, _userService);
        _log = new FileLogger(logPath);
    }
    public static void RunApp(string logPath, string dbPath)
    {
        InitializeServices(logPath, dbPath);
        try
        {
            _userService.CreateSuperUser();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"===== MAIN MENU =====");
            Console.WriteLine("1. Log in to the system");
            Console.WriteLine("2. Exit");
            Console.Write("Select an action: ");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Login();
                    break;
                case "2":
                    return;
                default:
                    Console.WriteLine("Incorrect choice");
                    break;
            }
        }
    }

    private static void Login()
    {
        var user = ShowLoginScreen();
        if (user != null)
        {
            if (user.Role == UserRoles.Manager)
            {
                ShowManagerMenu(user);
            }
            if (user.Role == UserRoles.Employee)
            {
                ShowEmployeeMenu(user);
            }
        }
        else
        {
            Console.WriteLine("Invalid credentials");
        }
    }

    private static User? ShowLoginScreen()
    {
        Console.Clear();
        Console.WriteLine("Login: ");
        var login = Console.ReadLine();
        Console.WriteLine("Password: ");
        var password = Console.ReadLine();
        try
        {
            var user = _authService.Login(login, password);
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
        return null;
    }

    private static void ShowManagerMenu(User user)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"===== MANAGER MENU ({user.Login}) =====");
            Console.WriteLine("1. Show all tasks");
            Console.WriteLine("2. Create a new task");
            Console.WriteLine("3. Assign a task to an employee");
            Console.WriteLine("4. Register a new employee");
            Console.WriteLine("5. Log out of the system");
            Console.Write("Select an action: ");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ViewAllTasks(user);
                    break;
                case "2":
                    CreateTask(user);
                    break;
                case "3":
                    AssignTaskToEmployee(user);
                    break;
                case "4":
                    RegisterEmployee(user);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Incorrect choice");
                    break;
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    private static void ViewAllTasks(User user)
    {
        if (user.Role != UserRoles.Manager)
        {
            Console.WriteLine("The user is not a manager");
            return;
        }
        Console.WriteLine("\n---- ALL TASKS ----");
        var tasks = _projectService.GetAllTasks();
        if (!tasks.Any())
        {
            Console.WriteLine("No tasks yet");
            return;
        }
        foreach (var task in tasks)
        {
            Console.WriteLine($"ID: {task.TaskID}");
            Console.WriteLine($"Project: {task.ProjectID}");
            Console.WriteLine($"Title: {task.Title}");
            Console.WriteLine($"Status: {task.Status}");
            Console.WriteLine($"Employee: {task.EmployeeLogin ?? "Not assigned"}");
            Console.WriteLine($"Description: {task.Description}");
            Console.WriteLine("-----------------------------------");
        }
    }

    private static void CreateTask(User user)
    {
        if (user.Role != UserRoles.Manager)
        {
            Console.WriteLine("The user is not a manager");
            return;
        }

        Console.WriteLine("\n---- CREATING A NEW TASK ----");
        string title = "";
        do
        {
            Console.Write("Task title: ");
            title = Console.ReadLine() ?? "";
        } while (title == "");

        Console.Write("Task description: ");
        var description = Console.ReadLine();

        string projectID = "";
        do
        {
            Console.Write("Project ID: ");
            projectID = Console.ReadLine() ?? "";
        } while (projectID == "" && !projectID.All(char.IsDigit));

        var task = new TaskForEmployee
        {
            TaskID = Guid.NewGuid(),
            Title = title,
            Description = description ?? "",
            Status = TaskForEmployeeStatus.ToDo,
            ProjectID = projectID,
            EmployeeLogin = null
        };
        try
        {
            _projectService.CreateTask(task);
            Console.WriteLine("\nThe task has been successfully created");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }

    private static void AssignTaskToEmployee(User user)
    {
        if (user.Role != UserRoles.Manager)
        {
            Console.WriteLine("The user is not a manager");
            return;
        }
        Console.WriteLine("\n---- ASSIGNING A TASK TO AN EMPLOYEE ----");
        var unassignedTasks = _projectService.GetAllTasks().Where(t => string.IsNullOrEmpty(t.EmployeeLogin)).ToList();
        if (!unassignedTasks.Any())
        {
            Console.WriteLine("There are no tasks without an assignment");
            return;
        }
        Console.WriteLine("\nTasks without an assignment:");
        foreach (var task in unassignedTasks)
        {
            Console.WriteLine($"ID: {task.TaskID}, Title: {task.Title}");
        }
        Console.Write("Enter the task ID for which you want to assign an employee: ");
        var taskId = Console.ReadLine() ?? "";
        Console.WriteLine("\nEmployees:");
        var employees = _userService.GetAllEmployees();
        foreach (var employee in employees)
        {
            Console.WriteLine($"Login: {employee.Login}");
        }
        Console.Write("Enter the employee's login: ");
        var employeeLogin = Console.ReadLine() ?? "";
        try
        {
            _projectService.AssignEmployeeToTask(Guid.Parse(taskId), employeeLogin);
            Console.WriteLine("\nThe task has been successfully assigned");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }

    private static void RegisterEmployee(User user)
    {
        if (user.Role != UserRoles.Manager)
        {
            Console.WriteLine("The user is not a manager");
            return;
        }
        Console.WriteLine("\n---- REGISTRATION OF A NEW EMPLOYEE ----");
        Console.Write("Enter the new employee's login: ");
        var login = Console.ReadLine() ?? "";
        Console.Write("Enter the new employee's password: ");
        var password = Console.ReadLine() ?? "";
        var new_user = new User
        {
            Login = login,
            Password = password,
            Role = UserRoles.Employee
        };
        try
        {
            _userService.RegisterUser(new_user);
            Console.WriteLine("\nThe employee has been successfully registered");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }

    private static void ShowEmployeeMenu(User user)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"===== EMPLOYEE MENU ({user.Login}) =====");
            Console.WriteLine("1. Show your tasks");
            Console.WriteLine("2. Change the issue status");
            Console.WriteLine("3. Log out of the system");
            Console.Write("Select an action: ");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ViewMyTasks(user);
                    break;
                case "2":
                    UpdateTaskStatus(user);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Incorrect choice");
                    break;
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    private static void ViewMyTasks(User user)
    {
        if (user.Role != UserRoles.Employee)
        {
            Console.WriteLine("The user is not an employee, he has no tasks to view");
            return;
        }
        Console.WriteLine("\n---- ALL MY TASKS ----");
        var tasks = _projectService.GetEmployeeTasks(user.Login);
        if (!tasks.Any())
        {
            Console.WriteLine("You have no tasks.");
            return;
        }
        foreach (var task in tasks)
        {
            Console.WriteLine($"ID: {task.TaskID}");
            Console.WriteLine($"Project ID: {task.ProjectID}");
            Console.WriteLine($"Title: {task.Title}");
            Console.WriteLine($"Status: {task.Status}");
            Console.WriteLine($"Description: {task.Description}");
            Console.WriteLine("-----------------------------------");
        }
    }

    private static void UpdateTaskStatus(User user)
    {
        if (user.Role != UserRoles.Employee)
        {
            Console.WriteLine("The user is not an employee, he has no tasks to change their status");
            return;
        }
        Console.WriteLine("\n---- CHANGING THE STATUS OF A TASK ----");
        var myTasks = _projectService.GetEmployeeTasks(user.Login);
        if (!myTasks.Any())
        {
            Console.WriteLine("You have no tasks.");
            return;
        }
        Console.WriteLine("\nYour tasks:");
        foreach (var task in myTasks)
        {
            Console.WriteLine($"ID: {task.TaskID}, Title: {task.Title}");
        }
        Console.Write("Enter the task ID you want to change status: ");
        var taskId = Console.ReadLine() ?? "";
        Console.WriteLine("Selectable statuses:");
        Console.WriteLine("0 - To Do");
        Console.WriteLine("1 - In Progress");
        Console.WriteLine("2 - Done");
        try
        {
            Console.Write("Select status (0-2): ");
            var statusChoice = Console.ReadLine() ?? "";
            TaskForEmployeeStatus status;
            switch (statusChoice)
            {
                case "0":
                    status = TaskForEmployeeStatus.ToDo;
                    break;
                case "1":
                    status = TaskForEmployeeStatus.InProgress;
                    break;
                case "2":
                    status = TaskForEmployeeStatus.Done;
                    break;
                default:
                    Console.WriteLine("Incorrect choice");
                    return;
            }
            _projectService.UpdateTaskStatus(Guid.Parse(taskId), status);
            _log.Log($"{user.Login} changed the status of the task {taskId} to {status.ToString()}");
            Console.WriteLine("\nThe task status has been successfully updated");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
    }
}