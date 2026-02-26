using System;
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
            System.Console.WriteLine("✓ Default data already exists. Skipping seed.");
            return;
        }

        System.Console.WriteLine("Seeding default host employees...");

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
            System.Console.WriteLine("✓ Receptionist account created.");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"✗ Failed to create Receptionist account: {ex.Message}");
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
            System.Console.WriteLine("✓ Staff account created.");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"✗ Failed to create Staff account: {ex.Message}");
        }

        System.Console.WriteLine("✓ Seeding completed.");
    }
}