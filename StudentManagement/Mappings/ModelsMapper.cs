using AutoMapper;
using StudentManagement.Dtos;
using StudentManagement.Entities;

namespace StudentManagement.Mappings
{
    public class ModelsMapper:Profile
    {
        public ModelsMapper()
        {
            CreateMap<Student, StudentDto>().ReverseMap();
        }
    }
}
