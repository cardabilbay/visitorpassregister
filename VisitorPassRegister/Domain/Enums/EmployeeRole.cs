namespace VisitorPassRegister.Domain.Enums;

/// <summary>
/// Represents the role of an employee in the system.
/// </summary>
public enum EmployeeRole
{
    /// <summary>
    /// Receptionist role - manages visitor check-in/check-out and pass registration.
    /// </summary>
    Receptionist = 1,

    /// <summary>
    /// Staff role - can be visited by guests.
    /// </summary>
    Staff = 2
}
