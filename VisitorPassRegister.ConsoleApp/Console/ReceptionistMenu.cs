using System;
using VisitorPassRegister.ConsoleApp.Models;

namespace VisitorPassRegister.ConsoleApp.Console;

public class ReceptionistMenu
{
    private readonly HostEmployee _currentUser;

    public ReceptionistMenu(HostEmployee currentUser)
    {
        _currentUser = currentUser;
    }

    public async Task<bool> ShowMenuAsync()
    {
        while (true)
        {
            ConsoleUI.PrintHeader($"Receptionist Menu - {_currentUser.FullName}");

            ConsoleUI.PrintSection("Main Menu");
            ConsoleUI.PrintMenu(
                "View Current User",
                "Check-In Visitor (Coming Soon)",
                "Check-Out Visitor (Coming Soon)",
                "View Active Visits (Coming Soon)",
                "Logout"
            );

            var choice = ConsoleUI.GetMenuChoice(5);

            switch (choice)
            {
                case 1:
                    ShowCurrentUser();
                    break;
                case 2:
                    ConsoleUI.PrintInfo("Check-In Visitor feature coming soon.");
                    ConsoleUI.PressAnyKeyToContinue();
                    break;
                case 3:
                    ConsoleUI.PrintInfo("Check-Out Visitor feature coming soon.");
                    ConsoleUI.PressAnyKeyToContinue();
                    break;
                case 4:
                    ConsoleUI.PrintInfo("View Active Visits feature coming soon.");
                    ConsoleUI.PressAnyKeyToContinue();
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