using Microsoft.EntityFrameworkCore;
using PJSoulsAPI.Models;
using PJSoulsAPI.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddDbContext<PjsoulsContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("pjsouls")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aplicar CORS aqu� antes de Authorization
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.MapCharacterEndpoints();

app.Run();
