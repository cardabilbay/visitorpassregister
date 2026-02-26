using System;
using VisitorPassRegister.ConsoleApp.Models;

namespace VisitorPassRegister.ConsoleApp.Console;

public class StaffMenu
{
    private readonly HostEmployee _currentUser;

    public StaffMenu(HostEmployee currentUser)
    {
        _currentUser = currentUser;
    }

    public async Task<bool> ShowMenuAsync()
    {
        while (true)
        {
            ConsoleUI.PrintHeader($"Staff Menu - {_currentUser.FullName}");

            ConsoleUI.PrintSection("Main Menu");
            ConsoleUI.PrintMenu(
                "View Current User",
                "View My Hosted Visits (Coming Soon)",
                "Check Visitor Status (Coming Soon)",
                "Logout"
            );

            var choice = ConsoleUI.GetMenuChoice(4);

            switch (choice)
            {
                case 1:
                    ShowCurrentUser();
                    break;
                case 2:
                    ConsoleUI.PrintInfo("View My Hosted Visits feature coming soon.");
                    ConsoleUI.PressAnyKeyToContinue();
                    break;
                case 3:
                    ConsoleUI.PrintInfo("Check Visitor Status feature coming soon.");
                    ConsoleUI.PressAnyKeyToContinue();
                    break;
                case 4:
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