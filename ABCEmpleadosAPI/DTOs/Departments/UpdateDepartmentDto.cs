using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.API.DTOs.Departments;

public class UpdateDepartmentDto
{
    [Required(ErrorMessage = "El nombre del departamento es obligatorio.")]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El código del departamento es obligatorio.")]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }
    public bool IsActive { get; set; }
}