namespace StudentManagement.Dtos
{
    public class LoginDto
    {
        public LoginDto()
        {
            Email="";
            Pass="";
            Role="user";

        }
        public int Id { get; set; }
        public string Email {  get; set; }
        public string Pass { get; set; }
        public string? Token { get; set; }
        public string Role { get; set; }

    }
}
