using EmployeesManagement.API.Data;
using EmployeesManagement.API.DTOs.Departments;
using EmployeesManagement.API.Models;
using EmployeesManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagement.API.Services;

public class DepartmentsService : IDepartmentsService
{
    private readonly ApplicationDbContext _context;

    public DepartmentsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        return await _context.Departments
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Code = d.Code,
                Description = d.Description,
                IsActive = d.IsActive,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<DepartmentDto?> GetByIdAsync(long id)
    {
        var d = await _context.Departments.FindAsync(id);
        if (d == null) return null;

        return new DepartmentDto
        {
            Id = d.Id,
            Name = d.Name,
            Code = d.Code,
            Description = d.Description,
            IsActive = d.IsActive,
            CreatedAt = d.CreatedAt,
            UpdatedAt = d.UpdatedAt
        };
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto createDto)
    {
        var department = new Department
        {
            Name = createDto.Name,
            Code = createDto.Code,
            Description = createDto.Description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
            Code = department.Code,
            Description = department.Description,
            IsActive = department.IsActive,
            CreatedAt = department.CreatedAt,
            UpdatedAt = department.UpdatedAt
        };
    }

    public async Task<DepartmentDto?> UpdateAsync(long id, UpdateDepartmentDto updateDto)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null) return null;

        department.Name = updateDto.Name;
        department.Code = updateDto.Code;
        department.Description = updateDto.Description;
        department.IsActive = updateDto.IsActive;
        department.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
            Code = department.Code,
            Description = department.Description,
            IsActive = department.IsActive,
            CreatedAt = department.CreatedAt,
            UpdatedAt = department.UpdatedAt
        };
    }

    public async Task<bool> UpdateStatusAsync(long id, bool isActive)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null) return false;

        department.IsActive = isActive;
        department.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}