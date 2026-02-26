using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VisitorPassRegister.ConsoleApp.Data;
using VisitorPassRegister.ConsoleApp.Repositories;
using VisitorPassRegister.ConsoleApp.Services;

// Load configuration
var basePath = AppContext.BaseDirectory;
var configuration = new ConfigurationBuilder()
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Set up dependency injection
var services = new ServiceCollection();

// Register DbContext
var connectionString = configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Register Repositories
services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
services.AddScoped<IVisitorRepository, VisitorRepository>();
services.AddScoped<IHostEmployeeRepository, HostEmployeeRepository>();
services.AddScoped<IVisitRecordRepository, VisitRecordRepository>();

// Register Services
services.AddScoped<VisitorService>();
services.AddScoped<HostEmployeeService>();
services.AddScoped<VisitRecordService>();

var serviceProvider = services.BuildServiceProvider();

// Test DbContext resolution
using (var scope = serviceProvider.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Apply migrations
    Console.WriteLine("Applying database migrations...");
    dbContext.Database.Migrate();
    Console.WriteLine("✓ Database migrations applied successfully.");

    // Verify repository resolution
    var visitorRepo = scope.ServiceProvider.GetRequiredService<IVisitorRepository>();
    var hostRepo = scope.ServiceProvider.GetRequiredService<IHostEmployeeRepository>();
    var visitRepo = scope.ServiceProvider.GetRequiredService<IVisitRecordRepository>();

    // Verify service resolution
    var visitorService = scope.ServiceProvider.GetRequiredService<VisitorService>();
    var hostService = scope.ServiceProvider.GetRequiredService<HostEmployeeService>();
    var visitService = scope.ServiceProvider.GetRequiredService<VisitRecordService>();

    Console.WriteLine("✓ Repositories and Services successfully resolved and configured.");
    Console.WriteLine("✓ Application is ready for further development.");
}
