using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Dtos
{
    public class StudentDto
    {
        public StudentDto()
        {
            Name="";
            Family="";
        }
        public int Id { get; set; }
        [Required(ErrorMessage ="نام دانشجو اجباری است") ]
        public string Name { get; set; }
        [Required(ErrorMessage ="فامیلی دانشجو اجباری است")]
        public string Family { get; set; }
    }
}
