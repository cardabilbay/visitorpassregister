using Microsoft.EntityFrameworkCore;
using VisitorPassRegister.ConsoleApp.Models;

namespace VisitorPassRegister.ConsoleApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Visitor> Visitors { get; set; } = null!;
    public DbSet<HostEmployee> HostEmployees { get; set; } = null!;
    public DbSet<VisitRecord> VisitRecords { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Visitor entity
        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.HasKey(v => v.Id);
            entity.Property(v => v.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(v => v.LastName).IsRequired().HasMaxLength(100);
            entity.Property(v => v.NationalIdNumber).IsRequired().HasMaxLength(50);
            entity.Property(v => v.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(v => v.CompanyName).IsRequired().HasMaxLength(150);
        });

        // Configure HostEmployee entity
        modelBuilder.Entity<HostEmployee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Department).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Role).IsRequired();
        });

        // Configure VisitRecord entity with relationships
        modelBuilder.Entity<VisitRecord>(entity =>
        {
            entity.HasKey(vr => vr.Id);
            
            entity.Property(vr => vr.Purpose).IsRequired().HasMaxLength(500);
            entity.Property(vr => vr.Status).IsRequired();
            entity.Property(vr => vr.PassNumber).IsRequired().HasMaxLength(50);
            entity.Property(vr => vr.CheckInTime).IsRequired();
            entity.Property(vr => vr.CheckOutTime);
            entity.Property(vr => vr.CreatedAt).IsRequired();

            // Configure relationship to Visitor
            entity.HasOne(vr => vr.Visitor)
                .WithMany()
                .HasForeignKey(vr => vr.VisitorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationship to HostEmployee
            entity.HasOne(vr => vr.HostEmployee)
                .WithMany()
                .HasForeignKey(vr => vr.HostEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
