using System;
using VisitorPassRegister.ConsoleApp.Enums;
using VisitorPassRegister.ConsoleApp.Models;
using VisitorPassRegister.ConsoleApp.Services;

namespace VisitorPassRegister.ConsoleApp.Console;

public class HostEmployeeManager
{
    private readonly HostEmployeeService _hostEmployeeService;

    public HostEmployeeManager(HostEmployeeService hostEmployeeService)
    {
        _hostEmployeeService = hostEmployeeService;
    }

    public async Task ShowMenuAsync()
    {
        while (true)
        {
            ConsoleUI.PrintHeader("Host Employee Management");
            ConsoleUI.PrintMenu(
                "List All Employees",
                "View Employee Details",
                "Create New Employee",
                "Update Employee",
                "Delete Employee",
                "Back to Main Menu"
            );

            var choice = ConsoleUI.GetMenuChoice(6);

            switch (choice)
            {
                case 1:
                    await ListEmployeesAsync();
                    break;
                case 2:
                    await ViewEmployeeAsync();
                    break;
                case 3:
                    await CreateEmployeeAsync();
                    break;
                case 4:
                    await UpdateEmployeeAsync();
                    break;
                case 5:
                    await DeleteEmployeeAsync();
                    break;
                case 6:
                    return;
            }
        }
    }

    private async Task ListEmployeesAsync()
    {
        ConsoleUI.PrintHeader("List All Employees");
        var employees = await _hostEmployeeService.GetAllEmployeesAsync();
        
        if (!employees.Any())
        {
            ConsoleUI.PrintInfo("No employees found.");
        }
        else
        {
            System.Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-15}", "ID", "Name", "Department", "Role");
            System.Console.WriteLine(new string('-', 70));
            
            foreach (var e in employees)
            {
                System.Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-15}", 
                    e.Id, 
                    e.FullName.Length > 20 ? e.FullName[..17] + "..." : e.FullName, 
                    e.Department.Length > 20 ? e.Department[..17] + "..." : e.Department,
                    e.Role);
            }
        }
        ConsoleUI.PressAnyKeyToContinue();
    }

    private async Task ViewEmployeeAsync()
    {
        ConsoleUI.PrintHeader("View Employee Details");
        var idStr = ConsoleUI.GetInput("Enter Employee ID: ");
        if (!int.TryParse(idStr, out int id))
        {
            ConsoleUI.PrintError("Invalid ID format.");
            ConsoleUI.PressAnyKeyToContinue();
            return;
        }

        var employee = await _hostEmployeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            ConsoleUI.PrintError("Employee not found.");
        }
        else
        {
            System.Console.WriteLine($"ID:         {employee.Id}");
            System.Console.WriteLine($"Username:   {employee.Username}");
            System.Console.WriteLine($"Full Name:  {employee.FullName}");
            System.Console.WriteLine($"Department: {employee.Department}");
            System.Console.WriteLine($"Role:       {employee.Role}");
        }
        ConsoleUI.PressAnyKeyToContinue();
    }

    private async Task CreateEmployeeAsync()
    {
        ConsoleUI.PrintHeader("Create New Employee");
        
        var employee = new HostEmployee();
        employee.Username = ConsoleUI.GetInput("Username: ");
        employee.Password = ConsoleUI.GetInput("Password: ");
        employee.FullName = ConsoleUI.GetInput("Full Name: ");
        employee.Department = ConsoleUI.GetInput("Department: ");
        
        System.Console.WriteLine("Select Role:");
        System.Console.WriteLine("1. Receptionist");
        System.Console.WriteLine("2. Staff");
        var roleChoice = ConsoleUI.GetMenuChoice(2);
        employee.Role = roleChoice == 1 ? Role.Receptionist : Role.Staff;

        try
        {
            await _hostEmployeeService.CreateEmployeeAsync(employee);
            ConsoleUI.PrintSuccess($"Employee '{employee.FullName}' created successfully with ID {employee.Id}.");
        }
        catch (Exception ex)
        {
            ConsoleUI.PrintError($"Failed to create employee: {ex.Message}");
        }
        ConsoleUI.PressAnyKeyToContinue();
    }

    private async Task UpdateEmployeeAsync()
    {
        ConsoleUI.PrintHeader("Update Employee");
        var idStr = ConsoleUI.GetInput("Enter Employee ID to update: ");
        if (!int.TryParse(idStr, out int id))
        {
            ConsoleUI.PrintError("Invalid ID format.");
            ConsoleUI.PressAnyKeyToContinue();
            return;
        }

        var employee = await _hostEmployeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            ConsoleUI.PrintError("Employee not found.");
            ConsoleUI.PressAnyKeyToContinue();
            return;
        }

        System.Console.WriteLine($"Updating details for {employee.FullName}. Leave blank to keep current value.");

        var username = ConsoleUI.GetInput($"Username ({employee.Username}): ");
        if (!string.IsNullOrWhiteSpace(username)) employee.Username = username;

        var password = ConsoleUI.GetInput("Password (leave blank to keep unchanged): ");
        if (!string.IsNullOrWhiteSpace(password)) employee.Password = password;

        var fullName = ConsoleUI.GetInput($"Full Name ({employee.FullName}): ");
        if (!string.IsNullOrWhiteSpace(fullName)) employee.FullName = fullName;

        var department = ConsoleUI.GetInput($"Department ({employee.Department}): ");
        if (!string.IsNullOrWhiteSpace(department)) employee.Department = department;

        System.Console.WriteLine($"Current Role: {employee.Role}");
        var changeRole = ConsoleUI.GetInput("Change Role? (y/n): ");
        if (changeRole.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            System.Console.WriteLine("Select New Role:");
            System.Console.WriteLine("1. Receptionist");
            System.Console.WriteLine("2. Staff");
            var roleChoice = ConsoleUI.GetMenuChoice(2);
            employee.Role = roleChoice == 1 ? Role.Receptionist : Role.Staff;
        }

        try
        {
            await _hostEmployeeService.UpdateEmployeeAsync(employee);
            ConsoleUI.PrintSuccess("Employee updated successfully.");
        }
        catch (Exception ex)
        {
            ConsoleUI.PrintError($"Failed to update employee: {ex.Message}");
        }
        ConsoleUI.PressAnyKeyToContinue();
    }

    private async Task DeleteEmployeeAsync()
    {
        ConsoleUI.PrintHeader("Delete Employee");
        var idStr = ConsoleUI.GetInput("Enter Employee ID to delete: ");
        if (!int.TryParse(idStr, out int id))
        {
            ConsoleUI.PrintError("Invalid ID format.");
            ConsoleUI.PressAnyKeyToContinue();
            return;
        }

        var confirm = ConsoleUI.GetInput($"Are you sure you want to delete employee ID {id}? (y/n): ");
        if (!confirm.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            ConsoleUI.PrintInfo("Deletion cancelled.");
            ConsoleUI.PressAnyKeyToContinue();
            return;
        }

        try
        {
            await _hostEmployeeService.DeleteEmployeeAsync(id);
            ConsoleUI.PrintSuccess("Employee deleted successfully.");
        }
        catch (Exception ex)
        {
            ConsoleUI.PrintError($"Failed to delete employee: {ex.Message}");
        }
        ConsoleUI.PressAnyKeyToContinue();
    }
}