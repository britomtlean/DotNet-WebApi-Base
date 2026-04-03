using WebApi2026.Context;
using WebApi2026.Interfaces;
using WebApi2026.Services;
using WebApi2026.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Mongo

// Configuração do appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings")
);

// Contexto do Mongo (substitui o AddDbContext<>())
builder.Services.AddSingleton<AppDbContext>();

// Services
builder.Services.AddScoped<IUsuarioService, UsuarioService>();



//MYSQL

// builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
