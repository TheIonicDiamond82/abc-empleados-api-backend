namespace EmployeesManagement.API.DTOs.Employees;

public class EmployeeDto
{
    public long Id { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public long DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public long PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}