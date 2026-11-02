using EmployeesManagement.API.DTOs.Departments;
using EmployeesManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesManagement.API.Controllers;

[Route("api/departments")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentsService _departmentsService;

    public DepartmentsController(IDepartmentsService departmentsService)
    {
        _departmentsService = departmentsService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll()
    {
        var departments = await _departmentsService.GetAllAsync();
        return Ok(departments);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<DepartmentDto>> GetById(long id)
    {
        var department = await _departmentsService.GetByIdAsync(id);
        if (department == null) return NotFound();
        return Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<DepartmentDto>> Create([FromBody] CreateDepartmentDto createDto)
    {
        var created = await _departmentsService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<DepartmentDto>> Update(long id, [FromBody] UpdateDepartmentDto updateDto)
    {
        var updated = await _departmentsService.UpdateAsync(id, updateDto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpPatch("{id:long}/status")]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] bool isActive)
    {
        var result = await _departmentsService.UpdateStatusAsync(id, isActive);
        if (!result) return NotFound();
        return NoContent();
    }
}