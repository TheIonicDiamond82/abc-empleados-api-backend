using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesManagement.API.Models;

[Table("departments")]
public class Department
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

    [Column("description")]
    public string? Description { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relaciones de navegación
    public ICollection<Position> Positions { get; set; } = new List<Position>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}