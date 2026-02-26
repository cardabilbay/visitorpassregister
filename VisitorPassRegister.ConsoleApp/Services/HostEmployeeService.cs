using VisitorPassRegister.ConsoleApp.Models;
using VisitorPassRegister.ConsoleApp.Repositories;

namespace VisitorPassRegister.ConsoleApp.Services;

public class HostEmployeeService
{
    private readonly IHostEmployeeRepository _hostEmployeeRepository;

    public HostEmployeeService(IHostEmployeeRepository hostEmployeeRepository)
    {
        _hostEmployeeRepository = hostEmployeeRepository;
    }

    public async Task<IEnumerable<HostEmployee>> GetAllEmployeesAsync()
    {
        return await _hostEmployeeRepository.GetAllAsync();
    }

    public async Task<HostEmployee?> GetEmployeeByIdAsync(int id)
    {
        return await _hostEmployeeRepository.GetByIdAsync(id);
    }

    public async Task<HostEmployee> CreateEmployeeAsync(HostEmployee employee)
    {
        ValidateEmployee(employee);

        // Check for duplicate username
        var allEmployees = await _hostEmployeeRepository.GetAllAsync();
        if (allEmployees.Any(e => e.Username.Equals(employee.Username, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"Username '{employee.Username}' is already taken.");
        }

        await _hostEmployeeRepository.AddAsync(employee);
        await _hostEmployeeRepository.SaveChangesAsync();
        return employee;
    }

    public async Task UpdateEmployeeAsync(HostEmployee employee)
    {
        ValidateEmployee(employee);

        var existingEmployee = await _hostEmployeeRepository.GetByIdAsync(employee.Id);
        if (existingEmployee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {employee.Id} not found.");
        }

        // Check if username is being changed to one that already exists
        if (!existingEmployee.Username.Equals(employee.Username, StringComparison.OrdinalIgnoreCase))
        {
            var allEmployees = await _hostEmployeeRepository.GetAllAsync();
            if (allEmployees.Any(e => e.Username.Equals(employee.Username, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Username '{employee.Username}' is already taken.");
            }
        }

        // Update fields
        existingEmployee.Username = employee.Username;
        existingEmployee.Password = employee.Password; // In real app, re-hash if changed
        existingEmployee.FullName = employee.FullName;
        existingEmployee.Department = employee.Department;
        existingEmployee.Role = employee.Role;

        _hostEmployeeRepository.Update(existingEmployee);
        await _hostEmployeeRepository.SaveChangesAsync();
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        var employee = await _hostEmployeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found.");
        }

        _hostEmployeeRepository.Delete(employee);
        await _hostEmployeeRepository.SaveChangesAsync();
    }

    private void ValidateEmployee(HostEmployee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.Username))
            throw new ArgumentException("Username is required.");
            
        if (string.IsNullOrWhiteSpace(employee.Password))
            throw new ArgumentException("Password is required.");
            
        if (string.IsNullOrWhiteSpace(employee.FullName))
            throw new ArgumentException("Full Name is required.");
            
        if (string.IsNullOrWhiteSpace(employee.Department))
            throw new ArgumentException("Department is required.");
    }
}