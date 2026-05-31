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

        var studentNames = new (string FirstName, string LastName)[]
        {
            ("Ava", "Thompson"),
            ("Liam", "Carter"),
            ("Maya", "Reed"),
            ("Noah", "Bennett"),
            ("Emma", "Hayes"),
            ("Ethan", "Parker"),
            ("Olivia", "Foster"),
            ("Lucas", "Bryant"),
            ("Sophia", "Long"),
            ("Mason", "Powell"),
            ("Isla", "Morgan"),
            ("Elijah", "Bell"),
            ("Harper", "Ross"),
            ("Logan", "Gray"),
            ("Amelia", "Wood"),
            ("James", "Cook"),
            ("Charlotte", "Brooks"),
            ("Benjamin", "Ward"),
            ("Ella", "Cooper"),
            ("Henry", "Bailey"),
            ("Scarlett", "Turner"),
            ("Daniel", "Morris"),
            ("Grace", "Murphy"),
            ("Sebastian", "Cox"),
            ("Layla", "Richardson"),
            ("Jack", "Howard"),
            ("Zoey", "Ward"),
            ("Owen", "Jenkins"),
            ("Nora", "Price"),
            ("Caleb", "Reed"),
            ("Stella", "Jenkins"),
            ("Wyatt", "Crawford"),
            ("Hannah", "Fisher"),
            ("Luke", "Sullivan"),
            ("Penelope", "Kim"),
            ("Julian", "Diaz"),
            ("Violet", "Stone"),
            ("Gabriel", "Marshall"),
            ("Chloe", "Ortiz"),
            ("Nathan", "Brooks")
        };

        var students = studentNames
            .Select((name, index) =>
            {
                return new Student
                {
                    FirstName = name.FirstName,
                    LastName = name.LastName,
                    Email = $"{name.FirstName.ToLowerInvariant()}.{name.LastName.ToLowerInvariant()}{index + 1}@classroom.local",
                    DateOfBirth = new DateOnly(1998 + ((index + 1) % 6), (((index + 1) % 12) + 1), (((index + 1) % 28) + 1))
                };
            })
            .ToList();

        context.Students.AddRange(students);
        context.SaveChanges();
    }
}
