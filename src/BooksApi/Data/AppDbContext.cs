using BooksApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Начальные книги (Seed data)
        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = 1,
                Title = "Чистый код",
                Author = "Роберт Мартин",
                Year = 2008,
                Price = 18500,
                Description = "Создание, анализ и рефакторинг чистого кода"
            },
            new Book
            {
                Id = 2,
                Title = "Идеальный программист",
                Author = "Роберт Мартин",
                Year = 2011,
                Price = 16000,
                Description = "Руководство по профессиональной разработке ПО"
            },
            new Book
            {
                Id = 3,
                Title = "Паттерны проектирования",
                Author = "Эрих Гамма",
                Year = 1994,
                Price = 22000,
                Description = "Классические объектно-ориентированные шаблоны"
            }
        );
    }
}
