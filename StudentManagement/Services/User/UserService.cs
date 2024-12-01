using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.Context;
using StudentManagement.Dtos;

using StudentManagement.Utility;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Resources;
using System.Security.Claims;
using System.Text;

namespace StudentManagement.Services.User;

    public class UserService:IUserService
    {
        public readonly DataContext _context;
        public readonly JwtSettings _setting;
        public readonly IMapper _mapper;
        
        
        public UserService(DataContext context ,IOptions<JwtSettings> setting , IMapper mapper)
        {
            _context = context;
            _setting= setting.Value;
            _mapper = mapper;
            



        }
        
        public bool ExistEmail(string email) => _context.Users.Any(u => u.Email.Trim()==email);
        public LoginDto Login(string email, string pass)
        {
            var hashPass = PasswordHelper.EncodeProSecurity(pass.Trim());
            var user = _context.Users.SingleOrDefault(u => u.Email==email && u.Pass == hashPass);
            if (user==null) {
                return new LoginDto();
            }
            var key = Encoding.ASCII.GetBytes(_setting.Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDecription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, user.Id.ToString()), new Claim(ClaimTypes.Role, user.Role.ToString()) }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _setting.Issuer,
                Audience = _setting.Audience,

            };
            var token = tokenHandler.CreateToken(tokenDecription);
            user.Token = tokenHandler.WriteToken(token);
            return _mapper.Map<LoginDto>(user);

        }
        public bool PasswordCorrect(string email, string pass) {
            var hashPass = PasswordHelper.EncodeProSecurity(pass.Trim());
             var user=_context.Users.Any(u => u.Email.Trim() == email.Trim() && u.Pass == hashPass);
            
            return user;

        }
        public bool Register(RegisterDto model) {
            var hashPass = PasswordHelper.EncodeProSecurity(model.Pass.Trim());
            Entities.User user = new() {
                Email = model.Email,
                Pass = hashPass,
                Role = "user"
            };
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception) {
                return false;
            }
        }


     

    }

