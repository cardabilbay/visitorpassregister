using VisitorPassRegister.ConsoleApp.Enums;

namespace VisitorPassRegister.ConsoleApp.Models;

public class HostEmployee
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public Role Role { get; set; }
}