using StudentManagement.Context;
using StudentManagement.Entities;

namespace StudentManagement.Services
{
    public class StudentService : IStudentService
    {
        private readonly DataContext _context;
        public StudentService(DataContext _context)
        {
            this._context = _context;
            
        }
        public List<Student> GetAll()
        {
            return _context.Students.ToList();
        }

        public Student GetById(int id)
        {
            var found = _context.Students.Where(x => x.Id == id).FirstOrDefault();
            if (found == null)
                return new();
            return found;

        }
        public bool Create(Student studen)
        {
            _context.Students.Add(studen);
            return Save();

        }

        public bool Delete(Student student)
        {
            _context.Students.Remove(student);
            return Save();

        }

        public bool Update(Student student)
        {
            _context.Update(student);
            return Save();
        }

        public bool Save() => _context.SaveChanges()>0 ?true : false;

        public bool CheckFamilyAndName(string family, string name) {
            return _context.Students.Any(x => x.Family==family && x.Name==name);
        }

        
    }
}
