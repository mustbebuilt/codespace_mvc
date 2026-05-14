using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;

namespace MyMvcApp.Controllers;

public class StudentsController : Controller
{
    private readonly ClassroomDbContext _db;

    public StudentsController(ClassroomDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _db.Students
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .ToListAsync();

        return View(students);
    }
}
