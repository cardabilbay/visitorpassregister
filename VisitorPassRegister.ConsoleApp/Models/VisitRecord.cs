using VisitorPassRegister.ConsoleApp.Enums;

namespace VisitorPassRegister.ConsoleApp.Models;

public class VisitRecord
{
    public int Id { get; set; }
    public int VisitorId { get; set; }
    public Visitor Visitor { get; set; } = null!;
    
    public int HostEmployeeId { get; set; }
    public HostEmployee HostEmployee { get; set; } = null!;
    
    public string Purpose { get; set; } = string.Empty;
    public VisitStatus Status { get; set; }
    public string PassNumber { get; set; } = string.Empty;
    
    public DateTime CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}