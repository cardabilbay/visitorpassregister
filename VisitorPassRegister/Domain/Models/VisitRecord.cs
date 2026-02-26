using VisitorPassRegister.Domain.Enums;

namespace VisitorPassRegister.Domain.Models;

/// <summary>
/// Represents a record of a visitor's visit to the organization.
/// </summary>
public class VisitRecord
{
    /// <summary>
    /// Gets or sets the unique identifier for the visit record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the visitor.
    /// </summary>
    public int VisitorId { get; set; }

    /// <summary>
    /// Gets or sets the visitor associated with this visit record.
    /// </summary>
    public Visitor Visitor { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ID of the host employee.
    /// </summary>
    public int HostEmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the host employee for this visit.
    /// </summary>
    public HostEmployee HostEmployee { get; set; } = null!;

    /// <summary>
    /// Gets or sets the purpose of the visit.
    /// </summary>
    public string Purpose { get; set; } = null!;

    /// <summary>
    /// Gets or sets the current status of the visit.
    /// </summary>
    public VisitStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the visitor pass number assigned to this visit.
    /// </summary>
    public string PassNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date and time when the visitor checked in.
    /// </summary>
    public DateTime? CheckInTime { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the visitor checked out (optional).
    /// </summary>
    public DateTime? CheckOutTime { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the visit record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
