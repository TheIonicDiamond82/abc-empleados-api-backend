using EmployeesManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagement.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasIndex(d => d.Code).IsUnique();
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasIndex(p => p.Code).IsUnique();
            
            // Llave alternativa requerida para soportar la llave foránea compuesta de Employee
            entity.HasAlternateKey(p => new { p.Id, p.DepartmentId });

            entity.HasOne(p => p.Department)
                  .WithMany(d => d.Positions)
                  .HasForeignKey(p => p.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasIndex(e => e.EmployeeNumber).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasOne(e => e.Department)
                  .WithMany(d => d.Employees)
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Restricción compuesta: asegura que el puesto pertenezca al departamento asignado
            entity.HasOne(e => e.Position)
                  .WithMany(p => p.Employees)
                  .HasForeignKey(e => new { e.PositionId, e.DepartmentId })
                  .HasPrincipalKey(p => new { p.Id, p.DepartmentId })
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}