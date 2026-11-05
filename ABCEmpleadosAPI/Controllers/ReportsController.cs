using EmployeesManagement.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagement.API.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReportsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("activity")]
    public async Task<IActionResult> GetActivity(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? movement = null)
    {
        var query = _context.Employees.AsQueryable();

        // Aplicar filtro de búsqueda si existe
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e => 
                e.FirstName.Contains(search) || 
                e.LastName.Contains(search) || 
                e.Email.Contains(search) ||
                e.EmployeeNumber.Contains(search));
        }

        var totalItems = await query.CountAsync();

        // Obtener los registros paginados
        var employees = await query
            .OrderByDescending(e => e.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Mapear los empleados al formato de actividad que espera el frontend
        var items = employees.Select(e => new
        {
            Id = e.Id,
            OccurredAt = e.CreatedAt,
            Movement = e.Status == "active" ? "Alta" : "Baja",
            Entity = "Empleado",
            Description = $"Empleado: {e.FirstName} {e.LastName} ({e.EmployeeNumber})",
            Actor = e.Email
        }).ToList();

        var totalPages = totalItems > 0 ? (int)Math.Ceiling(totalItems / (double)pageSize) : 1;

        var hiresCount = await _context.Employees.CountAsync(e => e.Status == "active");
        var terminationsCount = await _context.Employees.CountAsync(e => e.Status == "inactive" || e.Status == "terminated");

        var result = new
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            Summary = new
            {
                Hires = hiresCount,
                Changes = 0,
                Terminations = terminationsCount,
                Total = totalItems
            }
        };

        return Ok(result);
    }
}