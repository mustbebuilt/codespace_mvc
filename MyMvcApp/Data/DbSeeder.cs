using MyMvcApp.Models;

namespace MyMvcApp.Data;

public static class DbSeeder
{
    public static void Seed(ClassroomDbContext context)
    {
        if (context.Students.Any())
        {
            return;
        }

        var firstNames = new[]
        {
            "Ava", "Liam", "Maya", "Noah", "Emma",
            "Ethan", "Olivia", "Lucas", "Sophia", "Mason"
        };

        var lastNames = new[]
        {
            "Thompson", "Carter", "Reed", "Bennett", "Hayes",
            "Parker", "Foster", "Bryant", "Long", "Powell"
        };

        var students = Enumerable.Range(1, 40)
            .Select(i =>
            {
                var firstName = firstNames[(i - 1) % firstNames.Length];
                var lastName = lastNames[(i - 1) % lastNames.Length];

                return new Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = $"{firstName.ToLowerInvariant()}.{lastName.ToLowerInvariant()}{i}@classroom.local",
                    DateOfBirth = new DateOnly(1998 + (i % 6), ((i % 12) + 1), ((i % 28) + 1))
                };
            })
            .ToList();

        context.Students.AddRange(students);
        context.SaveChanges();
    }
}
