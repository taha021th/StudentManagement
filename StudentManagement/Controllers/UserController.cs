using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StudentManagement.Dtos;
using StudentManagement.Services.User;
using StudentManagement.Utility;

namespace StudentManagement.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _user;
        private readonly JwtSettings _setting;
        public UserController(IUserService user , IOptions<JwtSettings> setting)
        {
           _user=user;
           _setting=setting.Value;
        }
        [HttpPost("Register")]
        [ProducesResponseType(201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Register([FromBody] RegisterDto model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (_user.ExistEmail(model.Email)) {
                ModelState.AddModelError("model.Email", "ایمیل تکراری است");
                return BadRequest(ModelState);
            }

            if (_user.Register(model))
            {
                return StatusCode(201);

            }

            else {
                ModelState.AddModelError("", "خطای سرور مجدد تلاش کنید");
                return StatusCode(500, ModelState);
            }
            
        }

        [HttpPost("Login")]
        [ProducesResponseType(200, Type = typeof(LoginDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Login([FromBody] RegisterDto login) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_user.PasswordCorrect(login.Email, login.Pass)) {
                ModelState.AddModelError("model.Email", "کاربر یافت نشد");
                return BadRequest(ModelState);
            }
            var user = _user.Login(login.Email, login.Pass);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}
