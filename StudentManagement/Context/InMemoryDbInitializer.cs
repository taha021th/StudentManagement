using Microsoft.EntityFrameworkCore;
using StudentManagement.Entities;
using System.Security.Cryptography.X509Certificates;

namespace StudentManagement.Context
{
    public static class InMemoryDbInitializer
    {
        public static void Seed(DataContext context) {
            if (!context.Students.Any()) {
                context.Students.AddRange(
                    new Student { Id=1, Name = "دانیال",Family="اسدی"},
                    new Student { Id=2, Name="میلاد" , Family="حسینی"}
                    );
                context.SaveChanges();
            }
        }
    }
}
