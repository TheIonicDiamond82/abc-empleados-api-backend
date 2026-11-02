using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.API.DTOs.Employees;

public class UpdateEmployeeDto
{
    [Required(ErrorMessage = "El número de empleado es obligatorio.")]
    [MaxLength(50)]
    public string EmployeeNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "El ID del departamento es obligatorio.")]
    public long DepartmentId { get; set; }

    [Required(ErrorMessage = "El ID del puesto es obligatorio.")]
    public long PositionId { get; set; }

    [Required(ErrorMessage = "La fecha de contratación es obligatoria.")]
    public DateOnly HireDate { get; set; }

    [Required(ErrorMessage = "El estado es obligatorio.")]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    [Range(0, 99999999.99)]
    public decimal Salary { get; set; }
}