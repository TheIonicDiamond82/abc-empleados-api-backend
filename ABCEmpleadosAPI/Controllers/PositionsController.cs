using EmployeesManagement.API.DTOs.Positions;
using EmployeesManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesManagement.API.Controllers;

[Route("api/positions")]
[ApiController]
public class PositionsController : ControllerBase
{
    private readonly IPositionsService _positionsService;

    public PositionsController(IPositionsService positionsService)
    {
        _positionsService = positionsService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PositionDto>>> GetAll()
    {
        var positions = await _positionsService.GetAllAsync();
        return Ok(positions);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<PositionDto>> GetById(long id)
    {
        var position = await _positionsService.GetByIdAsync(id);
        if (position == null) return NotFound();
        return Ok(position);
    }

    [HttpPost]
    public async Task<ActionResult<PositionDto>> Create([FromBody] CreatePositionDto createDto)
    {
        try
        {
            var created = await _positionsService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<PositionDto>> Update(long id, [FromBody] UpdatePositionDto updateDto)
    {
        try
        {
            var updated = await _positionsService.UpdateAsync(id, updateDto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id:long}/status")]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] bool isActive)
    {
        var result = await _positionsService.UpdateStatusAsync(id, isActive);
        if (!result) return NotFound();
        return NoContent();
    }
}