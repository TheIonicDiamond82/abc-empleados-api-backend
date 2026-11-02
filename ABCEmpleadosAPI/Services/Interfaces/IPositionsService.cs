using EmployeesManagement.API.DTOs.Positions;

namespace EmployeesManagement.API.Services.Interfaces;

public interface IPositionsService
{
    Task<IEnumerable<PositionDto>> GetAllAsync();
    Task<PositionDto?> GetByIdAsync(long id);
    Task<PositionDto> CreateAsync(CreatePositionDto createDto);
    Task<PositionDto?> UpdateAsync(long id, UpdatePositionDto updateDto);
    Task<bool> UpdateStatusAsync(long id, bool isActive);
}