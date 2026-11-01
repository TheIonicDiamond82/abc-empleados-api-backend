using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesManagement.API.Models;

[Table("employees")]
public class Employee
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("employee_number")]
    [Required]
    [MaxLength(50)]
    public string EmployeeNumber { get; set; } = string.Empty;

    [Column("first_name")]
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Column("last_name")]
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Column("email")]
    [Required]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Column("phone")]
    [MaxLength(30)]
    public string? Phone { get; set; }

    [Column("department_id")]
    public long DepartmentId { get; set; }

    [Column("position_id")]
    public long PositionId { get; set; }

    [Column("hire_date")]
    public DateOnly HireDate { get; set; }

    [Column("status")]
    [MaxLength(50)]
    public string Status { get; set; } = "active";

    [Column("salary", TypeName = "numeric(18,2)")]
    public decimal Salary { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relaciones de navegación
    [ForeignKey("DepartmentId")]
    public Department Department { get; set; } = null!;

    [ForeignKey("PositionId")]
    public Position Position { get; set; } = null!;
}