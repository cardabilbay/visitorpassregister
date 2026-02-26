using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VisitorPassRegister.ConsoleApp.Data;

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

var serviceProvider = services.BuildServiceProvider();

// Test DbContext resolution
using (var scope = serviceProvider.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    Console.WriteLine("✓ DbContext successfully resolved and configured.");
    Console.WriteLine("✓ Application is ready for further development.");
}
