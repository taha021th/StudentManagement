using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Dtos
{
    public class RegisterDto
    {
        
        [Required(ErrorMessage ="ایمیل اجباری است")]
        [MaxLength(255)]
        public required string Email {  get; set; }
        [Required(ErrorMessage ="کلمه عبور اجباری است")]
        [MaxLength(90)]
        public required string Pass {  get; set; }
    }
}
