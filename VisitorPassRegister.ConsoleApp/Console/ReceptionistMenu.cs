using System;
using VisitorPassRegister.ConsoleApp.Models;

namespace VisitorPassRegister.ConsoleApp.Console;

public class ReceptionistMenu
{
    private readonly HostEmployee _currentUser;
    private readonly VisitorManager _visitorManager;
    private readonly HostEmployeeManager _hostEmployeeManager;
    private readonly VisitRecordManager _visitRecordManager;

    public ReceptionistMenu(
        HostEmployee currentUser,
        VisitorManager visitorManager,
        HostEmployeeManager hostEmployeeManager,
        VisitRecordManager visitRecordManager)
    {
        _currentUser = currentUser;
        _visitorManager = visitorManager;
        _hostEmployeeManager = hostEmployeeManager;
        _visitRecordManager = visitRecordManager;
    }

    public async Task<bool> ShowMenuAsync()
    {
        while (true)
        {
            ConsoleUI.PrintHeader($"Receptionist Menu - {_currentUser.FullName}");

            ConsoleUI.PrintSection("Main Menu");
            ConsoleUI.PrintMenu(
                "Manage Visitors",
                "Manage Host Employees",
                "Manage Visits",
                "View Current User",
                "Logout"
            );

            var choice = ConsoleUI.GetMenuChoice(5);

            switch (choice)
            {
                case 1:
                    await _visitorManager.ShowMenuAsync();
                    break;
                case 2:
                    await _hostEmployeeManager.ShowMenuAsync();
                    break;
                case 3:
                    await _visitRecordManager.ShowMenuAsync();
                    break;
                case 4:
                    ShowCurrentUser();
                    break;
                case 5:
                    ConsoleUI.PrintSuccess("Logging out...");
                    return false; // Exit menu, return to login
            }
        }
    }

    private void ShowCurrentUser()
    {
        ConsoleUI.PrintHeader("Current User Information");
        System.Console.WriteLine($"Name:       {_currentUser.FullName}");
        System.Console.WriteLine($"Username:   {_currentUser.Username}");
        System.Console.WriteLine($"Role:       {_currentUser.Role}");
        System.Console.WriteLine($"Department: {_currentUser.Department}");
        System.Console.WriteLine($"ID:         {_currentUser.Id}");
        ConsoleUI.PressAnyKeyToContinue();
    }
}