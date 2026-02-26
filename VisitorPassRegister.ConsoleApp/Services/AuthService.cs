using VisitorPassRegister.ConsoleApp.Models;

namespace VisitorPassRegister.ConsoleApp.Services;

public class AuthService
{
    private readonly HostEmployeeService _hostEmployeeService;

    public AuthService(HostEmployeeService hostEmployeeService)
    {
        _hostEmployeeService = hostEmployeeService;
    }

    public async Task<HostEmployee?> AuthenticateAsync(string username, string password)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.");

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required.");

        // Get all employees and find matching username
        var employees = await _hostEmployeeService.GetAllEmployeesAsync();
        var employee = employees.FirstOrDefault(e => e.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        if (employee == null)
            return null; // User not found

        // Check password (in production, this should use a proper hashing algorithm)
        if (employee.Password != password)
            return null; // Invalid password

        return employee; // Authentication successful
    }
}