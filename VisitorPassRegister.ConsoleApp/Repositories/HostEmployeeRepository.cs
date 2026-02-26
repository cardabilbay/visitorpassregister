using VisitorPassRegister.ConsoleApp.Data;
using VisitorPassRegister.ConsoleApp.Models;

namespace VisitorPassRegister.ConsoleApp.Repositories;

public class HostEmployeeRepository : GenericRepository<HostEmployee>, IHostEmployeeRepository
{
    public HostEmployeeRepository(AppDbContext context) : base(context)
    {
    }
}