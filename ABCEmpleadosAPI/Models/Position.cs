using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesManagement.API.Models;

[Table("positions")]
public class Position
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Column("code")]
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Column("department_id")]
    public long DepartmentId { get; set; }

    [Column("level")]
    [MaxLength(50)]
    public string? Level { get; set; }

    [Column("min_salary", TypeName = "numeric(18,2)")]
    public decimal MinSalary { get; set; }

    [Column("max_salary", TypeName = "numeric(18,2)")]
    public decimal MaxSalary { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relaciones de navegación
    [ForeignKey("DepartmentId")]
    public Department Department { get; set; } = null!;
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}