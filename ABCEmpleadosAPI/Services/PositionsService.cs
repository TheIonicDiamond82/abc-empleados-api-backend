using EmployeesManagement.API.Data;
using EmployeesManagement.API.DTOs.Positions;
using EmployeesManagement.API.Models;
using EmployeesManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagement.API.Services;

public class PositionsService : IPositionsService
{
    private readonly ApplicationDbContext _context;

    public PositionsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PositionDto>> GetAllAsync()
    {
        return await _context.Positions
            .Include(p => p.Department)
            .Select(p => new PositionDto
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                DepartmentId = p.DepartmentId,
                DepartmentName = p.Department.Name,
                Level = p.Level,
                MinSalary = p.MinSalary,
                MaxSalary = p.MaxSalary,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<PositionDto?> GetByIdAsync(long id)
    {
        var p = await _context.Positions
            .Include(p => p.Department)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (p == null) return null;

        return new PositionDto
        {
            Id = p.Id,
            Name = p.Name,
            Code = p.Code,
            DepartmentId = p.DepartmentId,
            DepartmentName = p.Department.Name,
            Level = p.Level,
            MinSalary = p.MinSalary,
            MaxSalary = p.MaxSalary,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        };
    }

    public async Task<PositionDto> CreateAsync(CreatePositionDto createDto)
    {
        var departmentExists = await _context.Departments.AnyAsync(d => d.Id == createDto.DepartmentId);
        if (!departmentExists)
        {
            throw new KeyNotFoundException($"El departamento con ID {createDto.DepartmentId} no existe.");
        }

        var position = new Position
        {
            Name = createDto.Name,
            Code = createDto.Code,
            DepartmentId = createDto.DepartmentId,
            Level = createDto.Level,
            MinSalary = createDto.MinSalary,
            MaxSalary = createDto.MaxSalary,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Positions.Add(position);
        await _context.SaveChangesAsync();

        await _context.Entry(position).Reference(p => p.Department).LoadAsync();

        return new PositionDto
        {
            Id = position.Id,
            Name = position.Name,
            Code = position.Code,
            DepartmentId = position.DepartmentId,
            DepartmentName = position.Department.Name,
            Level = position.Level,
            MinSalary = position.MinSalary,
            MaxSalary = position.MaxSalary,
            IsActive = position.IsActive,
            CreatedAt = position.CreatedAt,
            UpdatedAt = position.UpdatedAt
        };
    }

    public async Task<PositionDto?> UpdateAsync(long id, UpdatePositionDto updateDto)
    {
        var position = await _context.Positions.FindAsync(id);
        if (position == null) return null;

        var departmentExists = await _context.Departments.AnyAsync(d => d.Id == updateDto.DepartmentId);
        if (!departmentExists)
        {
            throw new KeyNotFoundException($"El departamento con ID {updateDto.DepartmentId} no existe.");
        }

        position.Name = updateDto.Name;
        position.Code = updateDto.Code;
        position.DepartmentId = updateDto.DepartmentId;
        position.Level = updateDto.Level;
        position.MinSalary = updateDto.MinSalary;
        position.MaxSalary = updateDto.MaxSalary;
        position.IsActive = updateDto.IsActive;
        position.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _context.Entry(position).Reference(p => p.Department).LoadAsync();

        return new PositionDto
        {
            Id = position.Id,
            Name = position.Name,
            Code = position.Code,
            DepartmentId = position.DepartmentId,
            DepartmentName = position.Department.Name,
            Level = position.Level,
            MinSalary = position.MinSalary,
            MaxSalary = position.MaxSalary,
            IsActive = position.IsActive,
            CreatedAt = position.CreatedAt,
            UpdatedAt = position.UpdatedAt
        };
    }

    public async Task<bool> UpdateStatusAsync(long id, bool isActive)
    {
        var position = await _context.Positions.FindAsync(id);
        if (position == null) return false;

        position.IsActive = isActive;
        position.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}