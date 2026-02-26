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
services.AddScoped<StartupSeeder>();
services.AddScoped<AuthService>();

var serviceProvider = services.BuildServiceProvider();

// Initialize application
using (var scope = serviceProvider.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Apply migrations
    Console.WriteLine("Applying database migrations...");
    dbContext.Database.Migrate();
    Console.WriteLine("✓ Database migrations applied successfully.\n");

    // Run seeding
    var seeder = scope.ServiceProvider.GetRequiredService<StartupSeeder>();
    await seeder.SeedDefaultDataAsync();
    Console.WriteLine();

    // Verify authentication setup
    var authService = scope.ServiceProvider.GetRequiredService<AuthService>();
    Console.WriteLine("Testing authentication...");
    
    // Test Receptionist login
    try
    {
        var receptionist = await authService.AuthenticateAsync("reception", "reception123");
        if (receptionist != null)
        {
            Console.WriteLine($"✓ Receptionist login successful: {receptionist.FullName} ({receptionist.Role})");
        }
        else
        {
            Console.WriteLine("✗ Receptionist login failed: Invalid credentials");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"✗ Receptionist login error: {ex.Message}");
    }

    // Test Staff login
    try
    {
        var staff = await authService.AuthenticateAsync("staff", "staff123");
        if (staff != null)
        {
            Console.WriteLine($"✓ Staff login successful: {staff.FullName} ({staff.Role})");
        }
        else
        {
            Console.WriteLine("✗ Staff login failed: Invalid credentials");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"✗ Staff login error: {ex.Message}");
    }

    // Test invalid login
    try
    {
        var invalid = await authService.AuthenticateAsync("invalid", "wrongpassword");
        if (invalid == null)
        {
            Console.WriteLine("✓ Invalid login correctly rejected");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"✗ Invalid login test error: {ex.Message}");
    }

    Console.WriteLine("\n✓ Application initialization completed successfully.");
    Console.WriteLine("✓ Application is ready for further development.");
}
