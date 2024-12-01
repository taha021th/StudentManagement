using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using StudentManagement.Resources;



var builder = WebApplication.CreateBuilder(args);

#region In memory matabase config
builder.Services.AddDbContext<DataContext>(options =>
    options.UseInMemoryDatabase("MyInMemoryDb"));
#endregion

#region Dependency


builder.Services.AddAutoMapper(typeof(ModelsMapper));
builder.Services.AddLocalization();
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddControllers();

#endregion

#region Add localization
builder.Services.AddLocalization();
var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("fa")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;


    options.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());
});

#endregion

#region Add swagger and dependency
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerStudentDocument>();
builder.Services.AddSwaggerGen();
#endregion

#region Jwt
var jwtSettingSection = builder.Configuration.GetSection("JWTSettings");
builder.Services.Configure<JwtSettings>(jwtSettingSection);
var jwtSettings = jwtSettingSection.Get<JwtSettings>();
if (jwtSettings == null)
{
    throw new InvalidOperationException("JWT settings are not configured properly.");
}

var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateIssuer = true,
        ValidAudience = jwtSettings.Audience,
        ValidateAudience = true,
        ValidateLifetime = false
    };

    
    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();

            // Resolve IStringLocalizer
            var localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<Resource>>();

            // Localize message
            var message = localizer["UnauthorizedMessage"].Value;

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message,
                status = 401
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    };
});


#endregion


var app = builder.Build();


#region Create record for database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    InMemoryDbInitializer.Seed(context);
}
#endregion

#region Read localization

var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);
#endregion


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
