using System;

namespace VisitorPassRegister.ConsoleApp.Console;

public static class ConsoleUI
{
    public static void PrintHeader(string title)
    {
        System.Console.Clear();
        System.Console.WriteLine("═══════════════════════════════════════════════════════════════");
        System.Console.WriteLine($"  {title}");
        System.Console.WriteLine("═══════════════════════════════════════════════════════════════\n");
    }

    public static void PrintSection(string section)
    {
        System.Console.WriteLine($"\n─ {section} ─\n");
    }

    public static void PrintSuccess(string message)
    {
        System.Console.WriteLine($"✓ {message}");
    }

    public static void PrintError(string message)
    {
        System.Console.WriteLine($"✗ {message}");
    }

    public static void PrintInfo(string message)
    {
        System.Console.WriteLine($"ℹ {message}");
    }

    public static string GetInput(string prompt)
    {
        System.Console.Write(prompt);
        return System.Console.ReadLine() ?? string.Empty;
    }

    public static string GetPassword(string prompt)
    {
        System.Console.Write(prompt);
        var password = string.Empty;
        ConsoleKey key;
        do
        {
            var keyInfo = System.Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password = password[0..^1];
                    System.Console.Write("\b \b");
                }
            }
            else if (char.IsControl(keyInfo.KeyChar))
            {
                // Allow only Enter, Escape, and Backspace
                if (key != ConsoleKey.Enter)
                    key = ConsoleKey.NoName;
            }
            else
            {
                password += keyInfo.KeyChar;
                System.Console.Write("*");
            }
        }
        while (key != ConsoleKey.Enter);

        System.Console.WriteLine();
        return password;
    }

    public static int GetMenuChoice(int maxOption)
    {
        while (true)
        {
            var input = GetInput("Enter your choice: ");
            if (int.TryParse(input, out var choice) && choice > 0 && choice <= maxOption)
            {
                return choice;
            }
            PrintError("Invalid choice. Please try again.");
        }
    }

    public static void PrintMenu(params string[] options)
    {
        for (int i = 0; i < options.Length; i++)
        {
            System.Console.WriteLine($"  {i + 1}. {options[i]}");
        }
        System.Console.WriteLine();
    }

    public static void PressAnyKeyToContinue()
    {
        System.Console.Write("\nPress any key to continue...");
        System.Console.ReadKey(true);
    }
}