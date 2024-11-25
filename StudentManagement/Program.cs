using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.Context;
using StudentManagement.Mappings;
using StudentManagement.Services;
using StudentManagement.Services.User;
using StudentManagement.Utility;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DataContext>(options =>
    options.UseInMemoryDatabase("MyInMemoryDb"));
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddAutoMapper(typeof(ModelsMapper));

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerStudentDocument>();
builder.Services.AddSwaggerGen();

#region Jwt
var jwtSettingSection = builder.Configuration.GetSection("JWTSettings");
builder.Services.Configure<JwtSettings>(jwtSettingSection);
var jwtSettings = jwtSettingSection.Get<JwtSettings>();
if (jwtSettings == null) { throw new InvalidOperationException("JWT settings are not configured properly."); }
var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}

).AddJwtBearer(option => {
    option.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateIssuer = true,
        ValidAudience = jwtSettings.Audience,
        ValidateAudience = true,
        ValidateLifetime=false

    };
});

#endregion

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    InMemoryDbInitializer.Seed(context);
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
