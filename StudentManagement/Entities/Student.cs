using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Entities
{
    public class Student
    {
        public Student()
        {
            Name="";
            Family="";
        }
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="نام دانشجو اجباری است")]
        public string Name {  get; set; }
        [Required(ErrorMessage = "فامیلی دانشجو اجباری است")]
        public string Family {  get; set; }
    }
}
