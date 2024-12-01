using StudentManagement.Dtos;

namespace StudentManagement.Services.User
{
    public interface IUserService
    {
        public bool Register(RegisterDto model);
        public LoginDto Login(string email, string pass);
        public bool ExistEmail(string email);
        public bool PasswordCorrect(string email, string pass);
        

    }
}
