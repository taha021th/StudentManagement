using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Context;
using StudentManagement.Mappings;
using StudentManagement.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DataContext>(options =>
    options.UseInMemoryDatabase("MyInMemoryDb"));
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddAutoMapper(typeof(ModelsMapper));
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



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

app.UseAuthorization();

app.MapControllers();

app.Run();
