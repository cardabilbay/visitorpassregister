using VisitorPassRegister.ConsoleApp.Enums;
using VisitorPassRegister.ConsoleApp.Models;

namespace VisitorPassRegister.ConsoleApp.Services;

public class StartupSeeder
{
    private readonly HostEmployeeService _hostEmployeeService;

    public StartupSeeder(HostEmployeeService hostEmployeeService)
    {
        _hostEmployeeService = hostEmployeeService;
    }

    public async Task SeedDefaultDataAsync()
    {
        // Check if any employees already exist
        var existingEmployees = await _hostEmployeeService.GetAllEmployeesAsync();
        if (existingEmployees.Any())
        {
            Console.WriteLine("✓ Default data already exists. Skipping seed.");
            return;
        }

        Console.WriteLine("Seeding default host employees...");

        // Seed Receptionist
        var receptionist = new HostEmployee
        {
            Username = "reception",
            Password = "reception123", // In production, this should be hashed
            FullName = "Reception Staff",
            Department = "Reception",
            Role = Role.Receptionist
        };

        try
        {
            await _hostEmployeeService.CreateEmployeeAsync(receptionist);
            Console.WriteLine("✓ Receptionist account created.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Failed to create Receptionist account: {ex.Message}");
        }

        // Seed Staff
        var staff = new HostEmployee
        {
            Username = "staff",
            Password = "staff123", // In production, this should be hashed
            FullName = "Staff Member",
            Department = "Operations",
            Role = Role.Staff
        };

        try
        {
            await _hostEmployeeService.CreateEmployeeAsync(staff);
            Console.WriteLine("✓ Staff account created.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Failed to create Staff account: {ex.Message}");
        }

        Console.WriteLine("✓ Seeding completed.");
    }
}