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
                if (!context.Users.Any()) {
                    context.Users.AddRange(
                        new User { Id=1, Email ="admin@gmail.com", Pass="2e99bf4e42962410038bc6fa4ce40d97", Role="admin" },
                        new User { Id=2, Email ="user@gmail.com", Pass="2e99bf4e42962410038bc6fa4ce40d97", Role="user" }
                        );
                    context.SaveChanges();
                }
            }



        }
    }
}
