namespace VisitorPassRegister.ConsoleApp.Models;

public class Visitor
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string NationalIdNumber { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";
}