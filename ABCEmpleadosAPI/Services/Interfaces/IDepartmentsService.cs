using EmployeesManagement.API.DTOs.Departments;

namespace EmployeesManagement.API.Services.Interfaces;

public interface IDepartmentsService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto?> GetByIdAsync(long id);
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto createDto);
    Task<DepartmentDto?> UpdateAsync(long id, UpdateDepartmentDto updateDto);
    Task<bool> UpdateStatusAsync(long id, bool isActive);
}