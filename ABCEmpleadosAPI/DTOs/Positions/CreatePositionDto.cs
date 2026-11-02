using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.API.DTOs.Positions;

public class CreatePositionDto
{
    [Required(ErrorMessage = "El nombre del puesto es obligatorio.")]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El código del puesto es obligatorio.")]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "El ID del departamento es obligatorio.")]
    public long DepartmentId { get; set; }

    [MaxLength(50)]
    public string? Level { get; set; }

    [Range(0, 99999999.99, ErrorMessage = "El salario mínimo debe ser mayor o igual a 0.")]
    public decimal MinSalary { get; set; }

    [Range(0, 99999999.99, ErrorMessage = "El salario máximo debe ser mayor o igual a 0.")]
    public decimal MaxSalary { get; set; }
}