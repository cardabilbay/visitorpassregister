namespace VisitorPassRegister.Domain.Enums;

/// <summary>
/// Represents the status of a visitor's visit record.
/// </summary>
public enum VisitStatus
{
    /// <summary>
    /// Visit has been registered but visitor has not checked in yet.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Visitor has checked in and is currently on premises.
    /// </summary>
    CheckedIn = 2,

    /// <summary>
    /// Visitor has checked out and left the premises.
    /// </summary>
    CheckedOut = 3,

    /// <summary>
    /// Visit has been cancelled.
    /// </summary>
    Cancelled = 4
}
