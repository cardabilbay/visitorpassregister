using VisitorPassRegister.Domain.Enums;

namespace VisitorPassRegister.Domain.Models;

/// <summary>
/// Represents an employee of the organization who hosts visitors or manages the reception.
/// </summary>
public class HostEmployee
{
    /// <summary>
    /// Gets or sets the unique identifier for the employee.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the employee's username for authentication.
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// Gets or sets the employee's password (should be hashed in production).
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// Gets or sets the employee's full name.
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the employee's department.
    /// </summary>
    public string Department { get; set; } = null!;

    /// <summary>
    /// Gets or sets the employee's role in the system.
    /// </summary>
    public EmployeeRole Role { get; set; }

    /// <summary>
    /// Gets or sets the collection of visit records where this employee is the host.
    /// </summary>
    public ICollection<VisitRecord> HostedVisits { get; set; } = new List<VisitRecord>();
}
