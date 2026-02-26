using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VisitorPassRegister.ConsoleApp.Console;
using VisitorPassRegister.ConsoleApp.Data;
using VisitorPassRegister.ConsoleApp.Enums;
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

// Register Console UI
services.AddScoped<LoginFlow>();
services.AddScoped<ReceptionistMenu>();
services.AddScoped<StaffMenu>();
services.AddScoped<VisitorManager>();
services.AddScoped<HostEmployeeManager>();
services.AddScoped<VisitRecordManager>();

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
}

// Main application loop
var running = true;
while (running)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var loginFlow = scope.ServiceProvider.GetRequiredService<LoginFlow>();
        var user = await loginFlow.ShowLoginScreenAsync();

        if (user != null)
        {
            // Route to appropriate menu based on role
            if (user.Role == Role.Receptionist)
            {
                var visitorManager = scope.ServiceProvider.GetRequiredService<VisitorManager>();
                var hostManager = scope.ServiceProvider.GetRequiredService<HostEmployeeManager>();
                var visitManager = scope.ServiceProvider.GetRequiredService<VisitRecordManager>();
                
                var receptionistMenu = new ReceptionistMenu(user, visitorManager, hostManager, visitManager);
                var continueSession = await receptionistMenu.ShowMenuAsync();
                if (!continueSession)
                {
                    // User logged out, return to login
                    continue;
                }
            }
            else if (user.Role == Role.Staff)
            {
                var staffMenu = new StaffMenu(user);
                var continueSession = await staffMenu.ShowMenuAsync();
                if (!continueSession)
                {
                    // User logged out, return to login
                    continue;
                }
            }
            else
            {
                ConsoleUI.PrintError("Unknown user role. Returning to login.");
                ConsoleUI.PressAnyKeyToContinue();
            }
        }
    }
}

