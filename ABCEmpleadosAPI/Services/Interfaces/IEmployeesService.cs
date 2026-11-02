using EmployeesManagement.API.DTOs.Employees;

namespace EmployeesManagement.API.Services.Interfaces;

public interface IEmployeesService
{
    Task<PagedResultDto<EmployeeDto>> GetPagedAsync(string? search, long? departmentId, long? positionId, string? status, int pageNumber, int pageSize);
    Task<EmployeeDto?> GetByIdAsync(long id);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto createDto);
    Task<EmployeeDto?> UpdateAsync(long id, UpdateEmployeeDto updateDto);
    Task<bool> UpdateStatusAsync(long id, string status);
}