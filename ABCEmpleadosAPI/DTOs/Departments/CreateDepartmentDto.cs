using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.API.DTOs.Departments;

public class CreateDepartmentDto
{
    [Required(ErrorMessage = "El nombre del departamento es obligatorio.")]
    [MaxLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El código del departamento es obligatorio.")]
    [MaxLength(50, ErrorMessage = "El código no puede superar los 50 caracteres.")]
    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }
}