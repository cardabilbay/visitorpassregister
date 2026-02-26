namespace VisitorPassRegister.Domain.Models;

/// <summary>
/// Represents a visitor to the organization.
/// </summary>
public class Visitor
{
    /// <summary>
    /// Gets or sets the unique identifier for the visitor.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the visitor's first name.
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the visitor's last name.
    /// </summary>
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the visitor's national ID number.
    /// </summary>
    public string NationalIdNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the visitor's phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the company the visitor represents.
    /// </summary>
    public string CompanyName { get; set; } = null!;

    /// <summary>
    /// Gets the full name of the visitor.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Gets or sets the collection of visit records associated with this visitor.
    /// </summary>
    public ICollection<VisitRecord> VisitRecords { get; set; } = new List<VisitRecord>();
}
