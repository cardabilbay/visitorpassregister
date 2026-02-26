using VisitorPassRegister.ConsoleApp.Data;
using VisitorPassRegister.ConsoleApp.Models;

namespace VisitorPassRegister.ConsoleApp.Repositories;

public class VisitRecordRepository : GenericRepository<VisitRecord>, IVisitRecordRepository
{
    public VisitRecordRepository(AppDbContext context) : base(context)
    {
    }
}