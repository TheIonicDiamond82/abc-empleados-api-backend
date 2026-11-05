using EmployeesManagement.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagement.API.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var totalEmployees = await _context.Employees.CountAsync();
        var activeEmployees = await _context.Employees.CountAsync(e => e.Status == "active");
        var totalDepartments = await _context.Departments.CountAsync();
        var totalPositions = await _context.Positions.CountAsync();

        // Fechas para cálculos del mes actual
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var hiresThisMonth = await _context.Employees
            .CountAsync(e => e.CreatedAt >= startOfMonth);

        var terminationsThisMonth = await _context.Employees
            .CountAsync(e => (e.Status == "inactive" || e.Status == "terminated") && e.UpdatedAt >= startOfMonth);

        // Cambios / Modificaciones del mes (empleados creados antes de este mes, actualizados en este mes y que siguen activos)
        var changes = await _context.Employees.CountAsync(e => e.Status == "active" && e.UpdatedAt > e.CreatedAt);

        // Desglose de empleados por departamento para las tarjetas o gráficas
        var departments = await _context.Departments
            .Select(d => new
            {
                DepartmentId = d.Id,
                Name = d.Name,
                Total = d.Employees.Count()
            })
            .ToListAsync();

        // Historial de actividad de los últimos 6 meses para la gráfica
        var past6Months = Enumerable.Range(0, 6)
            .Select(i => now.AddMonths(-5 + i)) // Orden cronológico ascendente
            .Select(d => new { Year = d.Year, Month = d.Month })
            .ToList();

        var employeesList = await _context.Employees
            .Select(e => new { e.CreatedAt, e.UpdatedAt, e.Status })
            .ToListAsync();

        var activityByMonth = past6Months.Select(m =>
        {
            var monthName = new DateTime(m.Year, m.Month, 1).ToString("MMM yyyy");
            var hires = employeesList.Count(e => e.CreatedAt.Year == m.Year && e.CreatedAt.Month == m.Month);
            var terminations = employeesList.Count(e => (e.Status == "inactive" || e.Status == "terminated") && e.UpdatedAt.Year == m.Year && e.UpdatedAt.Month == m.Month);

            return new
            {
                Month = monthName,
                Hires = hires,
                Terminations = terminations,
                Changes = changes
            };
        }).ToList();

        var summary = new
        {
            Totals = new
            {
                Employees = totalEmployees,
                ActiveEmployees = activeEmployees,
                Departments = totalDepartments,
                Positions = totalPositions,
                HiresThisMonth = hiresThisMonth,
                TerminationsThisMonth = terminationsThisMonth
            },
            ByDepartment = departments,
            ActivityByMonth = activityByMonth
        };

        return Ok(summary);
    }
}