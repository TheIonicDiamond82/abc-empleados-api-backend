using EmployeesManagement.API.Data;
using EmployeesManagement.API.DTOs.Employees;
using EmployeesManagement.API.Models;
using EmployeesManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagement.API.Services;

public class EmployeesService : IEmployeesService
{
    private readonly ApplicationDbContext _context;

    public EmployeesService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResultDto<EmployeeDto>> GetPagedAsync(string? search, long? departmentId, long? positionId, string? status, int pageNumber, int pageSize)
    {
        var query = _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .AsQueryable();

        // Filtro de búsqueda general (Nombre, Apellido, Email, Número de empleado)
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim().ToLower();
            query = query.Where(e => 
                e.FirstName.ToLower().Contains(searchTerm) ||
                e.LastName.ToLower().Contains(searchTerm) ||
                e.Email.ToLower().Contains(searchTerm) ||
                e.EmployeeNumber.ToLower().Contains(searchTerm));
        }

        // Filtros específicos
        if (departmentId.HasValue)
        {
            query = query.Where(e => e.DepartmentId == departmentId.Value);
        }

        if (positionId.HasValue)
        {
            query = query.Where(e => e.PositionId == positionId.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(e => e.Status.ToLower() == status.Trim().ToLower());
        }

        var totalCount = await query.CountAsync();

        // Paginación
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                EmployeeNumber = e.EmployeeNumber,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department.Name,
                PositionId = e.PositionId,
                PositionName = e.Position.Name,
                HireDate = e.HireDate,
                Status = e.Status,
                Salary = e.Salary,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            })
            .ToListAsync();

        return new PagedResultDto<EmployeeDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<EmployeeDto?> GetByIdAsync(long id)
    {
        var e = await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (e == null) return null;

        return MapToDto(e);
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto createDto)
    {
        // 1. Validar regla de negocio: El puesto debe pertenecer al departamento seleccionado
        await ValidatePositionBelongsToDepartmentAsync(createDto.PositionId, createDto.DepartmentId);

        var employee = new Employee
        {
            EmployeeNumber = createDto.EmployeeNumber,
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Email = createDto.Email,
            Phone = createDto.Phone,
            DepartmentId = createDto.DepartmentId,
            PositionId = createDto.PositionId,
            HireDate = createDto.HireDate,
            Status = "active", // Por defecto activo al crear
            Salary = createDto.Salary,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        // Recargar con relaciones para devolver los nombres mapeados
        await _context.Entry(employee).Reference(x => x.Department).LoadAsync();
        await _context.Entry(employee).Reference(x => x.Position).LoadAsync();

        return MapToDto(employee);
    }

    public async Task<EmployeeDto?> UpdateAsync(long id, UpdateEmployeeDto updateDto)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return null;

        // Validar regla de negocio de puesto/departamento
        await ValidatePositionBelongsToDepartmentAsync(updateDto.PositionId, updateDto.DepartmentId);

        employee.EmployeeNumber = updateDto.EmployeeNumber;
        employee.FirstName = updateDto.FirstName;
        employee.LastName = updateDto.LastName;
        employee.Email = updateDto.Email;
        employee.Phone = updateDto.Phone;
        employee.DepartmentId = updateDto.DepartmentId;
        employee.PositionId = updateDto.PositionId;
        employee.HireDate = updateDto.HireDate;
        employee.Status = updateDto.Status;
        employee.Salary = updateDto.Salary;
        employee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _context.Entry(employee).Reference(x => x.Department).LoadAsync();
        await _context.Entry(employee).Reference(x => x.Position).LoadAsync();

        return MapToDto(employee);
    }

    public async Task<bool> UpdateStatusAsync(long id, string status)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return false;

        employee.Status = status;
        employee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    // Método auxiliar para cumplir estrictamente con la regla de negocio
    private async Task ValidatePositionBelongsToDepartmentAsync(long positionId, long departmentId)
    {
        var position = await _context.Positions.FindAsync(positionId);
        if (position == null)
        {
            throw new KeyNotFoundException($"El puesto con ID {positionId} no existe.");
        }

        if (position.DepartmentId != departmentId)
        {
            throw new InvalidOperationException("El puesto seleccionado no pertenece al departamento asignado.");
        }
    }

    private static EmployeeDto MapToDto(Employee e) => new()
    {
        Id = e.Id,
        EmployeeNumber = e.EmployeeNumber,
        FirstName = e.FirstName,
        LastName = e.LastName,
        Email = e.Email,
        Phone = e.Phone,
        DepartmentId = e.DepartmentId,
        DepartmentName = e.Department?.Name ?? string.Empty,
        PositionId = e.PositionId,
        PositionName = e.Position?.Name ?? string.Empty,
        HireDate = e.HireDate,
        Status = e.Status,
        Salary = e.Salary,
        CreatedAt = e.CreatedAt,
        UpdatedAt = e.UpdatedAt
    };
}