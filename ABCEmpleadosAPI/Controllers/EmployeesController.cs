using EmployeesManagement.API.DTOs.Employees;
using EmployeesManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesManagement.API.Controllers;

[Route("api/employees")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeesService _employeesService;

    public EmployeesController(IEmployeesService employeesService)
    {
        _employeesService = employeesService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<EmployeeDto>>> GetPaged(
        [FromQuery] string? search,
        [FromQuery] long? departmentId,
        [FromQuery] long? positionId,
        [FromQuery] string? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100; // Límite máximo de seguridad por página

        var result = await _employeesService.GetPagedAsync(search, departmentId, positionId, status, pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<EmployeeDto>> GetById(long id)
    {
        var employee = await _employeesService.GetByIdAsync(id);
        if (employee == null) return NotFound();
        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create([FromBody] CreateEmployeeDto createDto)
    {
        try
        {
            var created = await _employeesService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<EmployeeDto>> Update(long id, [FromBody] UpdateEmployeeDto updateDto)
    {
        try
        {
            var updated = await _employeesService.UpdateAsync(id, updateDto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id:long}/status")]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] string status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return BadRequest(new { message = "El estado no puede estar vacío." });
        }

        var result = await _employeesService.UpdateStatusAsync(id, status);
        if (!result) return NotFound();
        return NoContent();
    }
}