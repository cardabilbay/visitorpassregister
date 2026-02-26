using VisitorPassRegister.ConsoleApp.Models;
using VisitorPassRegister.ConsoleApp.Repositories;

namespace VisitorPassRegister.ConsoleApp.Services;

public class VisitorService
{
    private readonly IVisitorRepository _visitorRepository;

    public VisitorService(IVisitorRepository visitorRepository)
    {
        _visitorRepository = visitorRepository;
    }

    public async Task<IEnumerable<Visitor>> GetAllVisitorsAsync()
    {
        return await _visitorRepository.GetAllAsync();
    }

    public async Task<Visitor?> GetVisitorByIdAsync(int id)
    {
        return await _visitorRepository.GetByIdAsync(id);
    }

    public async Task<Visitor> CreateVisitorAsync(Visitor visitor)
    {
        ValidateVisitor(visitor);
        
        // Check if visitor with same National ID already exists (simple check, could be optimized with repository method)
        var allVisitors = await _visitorRepository.GetAllAsync();
        if (allVisitors.Any(v => v.NationalIdNumber == visitor.NationalIdNumber))
        {
            throw new InvalidOperationException($"A visitor with National ID {visitor.NationalIdNumber} already exists.");
        }

        await _visitorRepository.AddAsync(visitor);
        await _visitorRepository.SaveChangesAsync();
        return visitor;
    }

    public async Task UpdateVisitorAsync(Visitor visitor)
    {
        ValidateVisitor(visitor);

        var existingVisitor = await _visitorRepository.GetByIdAsync(visitor.Id);
        if (existingVisitor == null)
        {
            throw new KeyNotFoundException($"Visitor with ID {visitor.Id} not found.");
        }

        // Update fields
        existingVisitor.FirstName = visitor.FirstName;
        existingVisitor.LastName = visitor.LastName;
        existingVisitor.NationalIdNumber = visitor.NationalIdNumber;
        existingVisitor.PhoneNumber = visitor.PhoneNumber;
        existingVisitor.CompanyName = visitor.CompanyName;

        _visitorRepository.Update(existingVisitor);
        await _visitorRepository.SaveChangesAsync();
    }

    public async Task DeleteVisitorAsync(int id)
    {
        var visitor = await _visitorRepository.GetByIdAsync(id);
        if (visitor == null)
        {
            throw new KeyNotFoundException($"Visitor with ID {id} not found.");
        }

        _visitorRepository.Delete(visitor);
        await _visitorRepository.SaveChangesAsync();
    }

    private void ValidateVisitor(Visitor visitor)
    {
        if (string.IsNullOrWhiteSpace(visitor.FirstName))
            throw new ArgumentException("First Name is required.");
        
        if (string.IsNullOrWhiteSpace(visitor.LastName))
            throw new ArgumentException("Last Name is required.");
            
        if (string.IsNullOrWhiteSpace(visitor.NationalIdNumber))
            throw new ArgumentException("National ID Number is required.");
            
        if (string.IsNullOrWhiteSpace(visitor.PhoneNumber))
            throw new ArgumentException("Phone Number is required.");
            
        if (string.IsNullOrWhiteSpace(visitor.CompanyName))
            throw new ArgumentException("Company Name is required.");
    }
}