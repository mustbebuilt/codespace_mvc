using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers;

public class HomeController : Controller
{
    private readonly ClassroomDbContext _db;

    public HomeController(ClassroomDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Search(string? q)
    {
        ViewData["Query"] = q;
        if (string.IsNullOrWhiteSpace(q))
            return View(Enumerable.Empty<Student>());

        var results = await _db.Students
            .Where(s => s.FirstName.Contains(q) || s.LastName.Contains(q) || s.Email.Contains(q))
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .ToListAsync();

        return View(results);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
