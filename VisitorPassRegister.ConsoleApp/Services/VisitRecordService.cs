using VisitorPassRegister.ConsoleApp.Enums;
using VisitorPassRegister.ConsoleApp.Models;
using VisitorPassRegister.ConsoleApp.Repositories;

namespace VisitorPassRegister.ConsoleApp.Services;

public class VisitRecordService
{
    private readonly IVisitRecordRepository _visitRecordRepository;
    private readonly IVisitorRepository _visitorRepository;
    private readonly IHostEmployeeRepository _hostEmployeeRepository;

    public VisitRecordService(
        IVisitRecordRepository visitRecordRepository,
        IVisitorRepository visitorRepository,
        IHostEmployeeRepository hostEmployeeRepository)
    {
        _visitRecordRepository = visitRecordRepository;
        _visitorRepository = visitorRepository;
        _hostEmployeeRepository = hostEmployeeRepository;
    }

    public async Task<IEnumerable<VisitRecord>> GetAllVisitsAsync()
    {
        return await _visitRecordRepository.GetAllAsync();
    }

    public async Task<VisitRecord?> GetVisitByIdAsync(int id)
    {
        return await _visitRecordRepository.GetByIdAsync(id);
    }

    public async Task<VisitRecord> CreateVisitAsync(VisitRecord visit)
    {
        ValidateVisit(visit);

        // Verify Visitor exists
        var visitor = await _visitorRepository.GetByIdAsync(visit.VisitorId);
        if (visitor == null)
        {
            throw new ArgumentException($"Visitor with ID {visit.VisitorId} does not exist.");
        }

        // Verify Host exists
        var host = await _hostEmployeeRepository.GetByIdAsync(visit.HostEmployeeId);
        if (host == null)
        {
            throw new ArgumentException($"Host Employee with ID {visit.HostEmployeeId} does not exist.");
        }

        visit.CreatedAt = DateTime.Now;
        // Ensure status is set appropriately if not provided
        if (visit.Status == 0) 
        {
            visit.Status = VisitStatus.Pending;
        }

        await _visitRecordRepository.AddAsync(visit);
        await _visitRecordRepository.SaveChangesAsync();
        return visit;
    }

    public async Task UpdateVisitAsync(VisitRecord visit)
    {
        ValidateVisit(visit);

        var existingVisit = await _visitRecordRepository.GetByIdAsync(visit.Id);
        if (existingVisit == null)
        {
            throw new KeyNotFoundException($"Visit Record with ID {visit.Id} not found.");
        }

        // Update fields
        existingVisit.VisitorId = visit.VisitorId;
        existingVisit.HostEmployeeId = visit.HostEmployeeId;
        existingVisit.Purpose = visit.Purpose;
        existingVisit.Status = visit.Status;
        existingVisit.PassNumber = visit.PassNumber;
        existingVisit.CheckInTime = visit.CheckInTime;
        existingVisit.CheckOutTime = visit.CheckOutTime;

        _visitRecordRepository.Update(existingVisit);
        await _visitRecordRepository.SaveChangesAsync();
    }

    public async Task DeleteVisitAsync(int id)
    {
        var visit = await _visitRecordRepository.GetByIdAsync(id);
        if (visit == null)
        {
            throw new KeyNotFoundException($"Visit Record with ID {id} not found.");
        }

        _visitRecordRepository.Delete(visit);
        await _visitRecordRepository.SaveChangesAsync();
    }

    private void ValidateVisit(VisitRecord visit)
    {
        if (visit.VisitorId <= 0)
            throw new ArgumentException("Valid Visitor ID is required.");
            
        if (visit.HostEmployeeId <= 0)
            throw new ArgumentException("Valid Host Employee ID is required.");
            
        if (string.IsNullOrWhiteSpace(visit.Purpose))
            throw new ArgumentException("Purpose of visit is required.");
            
        if (string.IsNullOrWhiteSpace(visit.PassNumber))
            throw new ArgumentException("Pass Number is required.");
    }
}