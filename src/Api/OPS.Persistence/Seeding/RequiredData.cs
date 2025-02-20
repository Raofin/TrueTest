using Microsoft.EntityFrameworkCore;
using OPS.Domain.Entities.Enum;

namespace OPS.Persistence.Seeding;

public static class RequiredData
{
    public static void SeedRequiredData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, RoleName = "Candidate" },
            new Role { Id = 2, RoleName = "Moderator" },
            new Role { Id = 3, RoleName = "Admin" }
        );

        modelBuilder.Entity<Difficulty>().HasData(
            new Difficulty { Id = 1, DifficultyName = "Easy" },
            new Difficulty { Id = 2, DifficultyName = "Medium" },
            new Difficulty { Id = 3, DifficultyName = "Hard" }
        );

        modelBuilder.Entity<QuestionType>().HasData(
            new QuestionType { Id = 1, Type = "Problem Solving" },
            new QuestionType { Id = 2, Type = "Written" },
            new QuestionType { Id = 3, Type = "MCQ" }
        );

        modelBuilder.Entity<ProgLanguage>().HasData(
            new ProgLanguage { Id = 1, Language = "Python" },
            new ProgLanguage { Id = 2, Language = "C" },
            new ProgLanguage { Id = 3, Language = "Cpp" },
            new ProgLanguage { Id = 4, Language = "Java" },
            new ProgLanguage { Id = 5, Language = "JavaScript" },
            new ProgLanguage { Id = 6, Language = "TypeScript" },
            new ProgLanguage { Id = 7, Language = "Csharp" },
            new ProgLanguage { Id = 8, Language = "Ruby" },
            new ProgLanguage { Id = 9, Language = "Go" },
            new ProgLanguage { Id = 10, Language = "PHP" }
        );
    }
}