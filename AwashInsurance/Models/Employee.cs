using System;
using System.Collections.Generic;

namespace AwashInsurance.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public DateOnly HireDate { get; set; }

    public string? JobTitle { get; set; }

    public string? Department { get; set; }

    public decimal? Salary { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }
}
