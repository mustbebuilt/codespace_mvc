namespace MyMvcApp.Models;

using System.ComponentModel.DataAnnotations;

public class Student
{
    public int StudentId { get; set; }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateOnly? DateOfBirth { get; set; }

    public DateTime EnrolledAt { get; set; }
}
