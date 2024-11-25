using StudentManagement.Entities;

namespace StudentManagement.Services
{
    public interface IStudentService
    {
        List<Student> GetAll();
        Student GetById(int id);
        bool Create(Student studen);
        bool Update(Student student);
        bool Delete(Student student);
        bool Save();
        bool CheckFamilyAndName(string family , string name);
    }
}
