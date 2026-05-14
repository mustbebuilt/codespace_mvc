using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;

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

    [HttpGet]
    public IActionResult Insert()
    {
        return View(new Student());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Insert(Student student)
    {
        if (!ModelState.IsValid) return View(student);

        student.EnrolledAt = DateTime.UtcNow;
        _db.Students.Add(student);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is null) return NotFound();
        return View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Student student)
    {
        if (!ModelState.IsValid) return View(student);

        var existing = await _db.Students.FindAsync(student.StudentId);
        if (existing is null) return NotFound();

        existing.FirstName = student.FirstName;
        existing.LastName = student.LastName;
        existing.Email = student.Email;
        existing.DateOfBirth = student.DateOfBirth;

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is not null)
        {
            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
