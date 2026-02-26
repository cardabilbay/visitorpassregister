using VisitorPassRegister.ConsoleApp.Models;
using VisitorPassRegister.ConsoleApp.Services;

namespace VisitorPassRegister.ConsoleApp.Console;

public class LoginFlow
{
    private readonly AuthService _authService;

    public LoginFlow(AuthService authService)
    {
        _authService = authService;
    }

    public async Task<HostEmployee?> ShowLoginScreenAsync()
    {
        ConsoleUI.PrintHeader("Visitor Pass Register - Login");

        while (true)
        {
            var username = ConsoleUI.GetInput("Username: ");
            var password = ConsoleUI.GetPassword("Password: ");

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ConsoleUI.PrintError("Username and password are required.");
                continue;
            }

            try
            {
                var user = await _authService.AuthenticateAsync(username, password);
                if (user != null)
                {
                    ConsoleUI.PrintSuccess($"Welcome, {user.FullName}!");
                    ConsoleUI.PressAnyKeyToContinue();
                    return user;
                }
                else
                {
                    ConsoleUI.PrintError("Invalid username or password. Please try again.");
                }
            }
            catch (Exception ex)
            {
                ConsoleUI.PrintError($"Authentication error: {ex.Message}");
            }
        }
    }
}