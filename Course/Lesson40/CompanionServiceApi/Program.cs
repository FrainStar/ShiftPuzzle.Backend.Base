using Microsoft.EntityFrameworkCore; 
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IUserManager>(provider =>
{
    var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
    optionsBuilder.UseSqlite("Data Source=UserDataBase.db"); 
    var taskContext = new UserContext(optionsBuilder.Options);
    taskContext.Database.EnsureCreated(); 
    IUserManager userManager = new UserManager(taskContext);

    return userManager;
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Сервис Попутчиков API", Version = "v1" });
}); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Сервис Попутчиков API v1"));
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();

app.Run();