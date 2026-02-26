using Microsoft.EntityFrameworkCore;
using VisitorPassRegister.Domain.Models;

namespace VisitorPassRegister.Data;

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

        // Visitor Configuration
        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.NationalIdNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(100);
        });

        // HostEmployee Configuration
        modelBuilder.Entity<HostEmployee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.Password).IsRequired(); // In a real app, ensure this stores a hash
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Department).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Role).IsRequired();
        });

        // VisitRecord Configuration
        modelBuilder.Entity<VisitRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Purpose).IsRequired().HasMaxLength(500);
            entity.Property(e => e.PassNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            // Relationships
            entity.HasOne(vr => vr.Visitor)
                  .WithMany(v => v.VisitRecords)
                  .HasForeignKey(vr => vr.VisitorId)
                  .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a visitor if they have records

            entity.HasOne(vr => vr.HostEmployee)
                  .WithMany(he => he.HostedVisits)
                  .HasForeignKey(vr => vr.HostEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict); // Prevent deleting an employee if they have hosted visits
        });
    }
}