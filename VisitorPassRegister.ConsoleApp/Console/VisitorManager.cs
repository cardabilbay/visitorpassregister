using System;
using VisitorPassRegister.ConsoleApp.Models;
using VisitorPassRegister.ConsoleApp.Services;

namespace VisitorPassRegister.ConsoleApp.Console;

public class VisitorManager
{
    private readonly VisitorService _visitorService;

    public VisitorManager(VisitorService visitorService)
    {
        _visitorService = visitorService;
    }

    public async Task ShowMenuAsync()
    {
        while (true)
        {
            ConsoleUI.PrintHeader("Visitor Management");
            ConsoleUI.PrintMenu(
                "List All Visitors",
                "View Visitor Details",
                "Create New Visitor",
                "Update Visitor",
                "Delete Visitor",
                "Back to Main Menu"
            );

            var choice = ConsoleUI.GetMenuChoice(6);

            switch (choice)
            {
                case 1:
                    await ListVisitorsAsync();
                    break;
                case 2:
                    await ViewVisitorAsync();
                    break;
                case 3:
                    await CreateVisitorAsync();
                    break;
                case 4:
                    await UpdateVisitorAsync();
                    break;
                case 5:
                    await DeleteVisitorAsync();
                    break;
                case 6:
                    return;
            }
        }
    }

    private async Task ListVisitorsAsync()
    {
        ConsoleUI.PrintHeader("List All Visitors");
        var visitors = await _visitorService.GetAllVisitorsAsync();
        
        if (!visitors.Any())
        {
            ConsoleUI.PrintInfo("No visitors found.");
        }
        else
        {
            System.Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-20}", "ID", "Name", "Company", "Phone");
            System.Console.WriteLine(new string('-', 70));
            
            foreach (var v in visitors)
            {
                System.Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-20}", 
                    v.Id, 
                    v.FullName.Length > 20 ? v.FullName[..17] + "..." : v.FullName, 
                    v.CompanyName.Length > 20 ? v.CompanyName[..17] + "..." : v.CompanyName,
                    v.PhoneNumber);
            }
        }
        ConsoleUI.PressAnyKeyToContinue();
    }

    private async Task ViewVisitorAsync()
    {
        ConsoleUI.PrintHeader("View Visitor Details");
        var idStr = ConsoleUI.GetInput("Enter Visitor ID: ");
        if (!int.TryParse(idStr, out int id))
        {
            ConsoleUI.PrintError("Invalid ID format.");
            ConsoleUI.PressAnyKeyToContinue();
            return;
        }

        var visitor = await _visitorService.GetVisitorByIdAsync(id);
        if (visitor == null)
        {
            ConsoleUI.PrintError("Visitor not found.");
        }
        else
        {
            System.Console.WriteLine($"ID:          {visitor.Id}");
            System.Console.WriteLine($"First Name:  {visitor.FirstName}");
            System.Console.WriteLine($"Last Name:   {visitor.LastName}");
            System.Console.WriteLine($"National ID: {visitor.NationalIdNumber}");
            System.Console.WriteLine($"Phone:       {visitor.PhoneNumber}");
            System.Console.WriteLine($"Company:     {visitor.CompanyName}");
        }
        ConsoleUI.PressAnyKeyToContinue();
    }

    private async Task CreateVisitorAsync()
    {
        ConsoleUI.PrintHeader("Create New Visitor");
        
        var visitor = new Visitor();
        visitor.FirstName = ConsoleUI.GetInput("First Name: ");
        visitor.LastName = ConsoleUI.GetInput("Last Name: ");
        visitor.NationalIdNumber = ConsoleUI.GetInput("National ID: ");
        visitor.PhoneNumber = ConsoleUI.GetInput("Phone Number: ");
        visitor.CompanyName = ConsoleUI.GetInput("Company Name: ");

        try
        {
            await _visitorService.CreateVisitorAsync(visitor);
            ConsoleUI.PrintSuccess($"Visitor '{visitor.FullName}' created successfully with ID {visitor.Id}.");
        }
        catch (Exception ex)
        {
            ConsoleUI.PrintError($"Failed to create visitor: {ex.Message}");
        }
        ConsoleUI.PressAnyKeyToContinue();
    }

    private async Task UpdateVisitorAsync()
    {
        ConsoleUI.PrintHeader("Update Visitor");
        var idStr = ConsoleUI.GetInput("Enter Visitor ID to update: ");
        if (!int.TryParse(idStr, out int id))
        {
            ConsoleUI.PrintError("Invalid ID format.");
            ConsoleUI.PressAnyKeyToContinue();
            return;
        }

        var visitor = await _visitorService.GetVisitorByIdAsync(id);
        if (visitor == null)
        {
            ConsoleUI.PrintError("Visitor not found.");
            ConsoleUI.PressAnyKeyToContinue();
            return;
        }

        System.Console.WriteLine($"Updating details for {visitor.FullName}. Leave blank to keep current value.");

        var firstName = ConsoleUI.GetInput($"First Name ({visitor.FirstName}): ");
        if (!string.IsNullOrWhiteSpace(firstName)) visitor.FirstName = firstName;

        var lastName = ConsoleUI.GetInput($"Last Name ({visitor.LastName}): ");
        if (!string.IsNullOrWhiteSpace(lastName)) visitor.LastName = lastName;

        var nationalId = ConsoleUI.GetInput($"National ID ({visitor.NationalIdNumber}): ");
        if (!string.IsNullOrWhiteSpace(nationalId)) visitor.NationalIdNumber = nationalId;

        var phone = ConsoleUI.GetInput($"Phone ({visitor.PhoneNumber}): ");
        if (!string.IsNullOrWhiteSpace(phone)) visitor.PhoneNumber = phone;

        var company = ConsoleUI.GetInput($"Company ({visitor.CompanyName}): ");
        if (!string.IsNullOrWhiteSpace(company)) visitor.CompanyName = company;

        try
        {
            await _visitorService.UpdateVisitorAsync(visitor);
            ConsoleUI.PrintSuccess("Visitor updated successfully.");
        }
        catch (Exception ex)
        {
            ConsoleUI.PrintError($"Failed to update visitor: {ex.Message}");
        }
        ConsoleUI.PressAnyKeyToContinue();
    }

    private async Task DeleteVisitorAsync()
    {
        ConsoleUI.PrintHeader("Delete Visitor");
        var idStr = ConsoleUI.GetInput("Enter Visitor ID to delete: ");
        if (!int.TryParse(idStr, out int id))
        {
            ConsoleUI.PrintError("Invalid ID format.");
            ConsoleUI.PressAnyKeyToContinue();
            return;
        }

        var confirm = ConsoleUI.GetInput($"Are you sure you want to delete visitor ID {id}? (y/n): ");
        if (!confirm.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            ConsoleUI.PrintInfo("Deletion cancelled.");
            ConsoleUI.PressAnyKeyToContinue();
            return;
        }

        try
        {
            await _visitorService.DeleteVisitorAsync(id);
            ConsoleUI.PrintSuccess("Visitor deleted successfully.");
        }
        catch (Exception ex)
        {
            ConsoleUI.PrintError($"Failed to delete visitor: {ex.Message}");
        }
        ConsoleUI.PressAnyKeyToContinue();
    }
}