using System;
using VisitorPassRegister.ConsoleApp.Enums;
using VisitorPassRegister.ConsoleApp.Models;
using VisitorPassRegister.ConsoleApp.Services;

namespace VisitorPassRegister.ConsoleApp.Console;

public class VisitRecordManager
{
    private readonly VisitRecordService _visitRecordService;

    public VisitRecordManager(VisitRecordService visitRecordService)
    {
        _visitRecordService = visitRecordService;
    }

    public async Task ShowMenuAsync()
    {
        while (true)
        {
            ConsoleUI.PrintHeader("Visit Record Management");
            ConsoleUI.PrintMenu(
                "List All Visits",
                "View Visit Details",
                "Create New Visit",
                "Back to Main Menu"
            );

            var choice = ConsoleUI.GetMenuChoice(4);

            switch (choice)
            {
                case 1:
                    await ListVisitsAsync();
                    break;
                case 2:
                    await ViewVisitAsync();
                    break;
                case 3:
                    await CreateVisitAsync();
                    break;
                case 4:
                    return;
            }
        }
    }

    private async Task ListVisitsAsync()
    {
        ConsoleUI.PrintHeader("List All Visits");
        var visits = await _visitRecordService.GetAllVisitsAsync();
        
        if (!visits.Any())
        {
            ConsoleUI.PrintInfo("No visits found.");
        }
        else
        {
            System.Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-15}", "ID", "Visitor ID", "Host ID", "Status");
            System.Console.WriteLine(new string('-', 70));
            
            foreach (var v in visits)
            {
                System.Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-15}", 
                    v.Id, 
                    v.VisitorId, 
                    v.HostEmployeeId,
                    v.Status);
            }
        }
        ConsoleUI.PressAnyKeyToContinue();
    }

    private async Task ViewVisitAsync()
    {
        ConsoleUI.PrintHeader("View Visit Details");
        var idStr = ConsoleUI.GetInput("Enter Visit ID: ");
        if (!int.TryParse(idStr, out int id))
        {
            ConsoleUI.PrintError("Invalid ID format.");
            ConsoleUI.PressAnyKeyToContinue();
            return;
        }

        var visit = await _visitRecordService.GetVisitByIdAsync(id);
        if (visit == null)
        {
            ConsoleUI.PrintError("Visit not found.");
        }
        else
        {
            System.Console.WriteLine($"ID:          {visit.Id}");
            System.Console.WriteLine($"Visitor ID:  {visit.VisitorId}");
            System.Console.WriteLine($"Host ID:     {visit.HostEmployeeId}");
            System.Console.WriteLine($"Purpose:     {visit.Purpose}");
            System.Console.WriteLine($"Pass Number: {visit.PassNumber}");
            System.Console.WriteLine($"Status:      {visit.Status}");
            System.Console.WriteLine($"Check-In:    {visit.CheckInTime}");
            System.Console.WriteLine($"Check-Out:   {visit.CheckOutTime}");
        }
        ConsoleUI.PressAnyKeyToContinue();
    }

    private async Task CreateVisitAsync()
    {
        ConsoleUI.PrintHeader("Create New Visit");
        
        var visit = new VisitRecord();
        
        var visitorIdStr = ConsoleUI.GetInput("Visitor ID: ");
        if (int.TryParse(visitorIdStr, out int visitorId))
        {
            visit.VisitorId = visitorId;
        }
        else
        {
            ConsoleUI.PrintError("Invalid Visitor ID.");
            ConsoleUI.PressAnyKeyToContinue();
            return;
        }

        var hostIdStr = ConsoleUI.GetInput("Host Employee ID: ");
        if (int.TryParse(hostIdStr, out int hostId))
        {
            visit.HostEmployeeId = hostId;
        }
        else
        {
            ConsoleUI.PrintError("Invalid Host ID.");
            ConsoleUI.PressAnyKeyToContinue();
            return;
        }

        visit.Purpose = ConsoleUI.GetInput("Purpose of Visit: ");
        visit.PassNumber = ConsoleUI.GetInput("Pass Number: ");
        
        visit.Status = VisitStatus.CheckedIn;
        visit.CheckInTime = DateTime.Now;

        try
        {
            await _visitRecordService.CreateVisitAsync(visit);
            ConsoleUI.PrintSuccess($"Visit created successfully with ID {visit.Id}.");
        }
        catch (Exception ex)
        {
            ConsoleUI.PrintError($"Failed to create visit: {ex.Message}");
        }
        ConsoleUI.PressAnyKeyToContinue();
    }
}