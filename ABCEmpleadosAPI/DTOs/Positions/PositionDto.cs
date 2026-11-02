namespace EmployeesManagement.API.DTOs.Positions;

public class PositionDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string? Level { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}