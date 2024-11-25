using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Entities
{
    public class User
    {
        public User()
        {
            Email = "";
            Pass = "";
            Token = "";
            Role ="user";

        }

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="نام کاربر اجباری است")]
        public  string Email { get; set; }
        [Required(ErrorMessage ="رمز عبور کاربر اجباری است")]
        public string Pass {  get; set; }        
        
        public string? Token { get; set; }
        [Required(ErrorMessage = "سطح دسترسی کاربر باید تایین شود")]
        public string  Role { get; set; }

    }
}
