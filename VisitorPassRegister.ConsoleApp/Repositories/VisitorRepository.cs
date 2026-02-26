using VisitorPassRegister.ConsoleApp.Data;
using VisitorPassRegister.ConsoleApp.Models;

namespace VisitorPassRegister.ConsoleApp.Repositories;

public class VisitorRepository : GenericRepository<Visitor>, IVisitorRepository
{
    public VisitorRepository(AppDbContext context) : base(context)
    {
    }
}