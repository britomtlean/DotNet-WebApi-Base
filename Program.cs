using WebApi2026.Context;
using WebApi2026.Interfaces;
using WebApi2026.Services;
using WebApi2026.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


//////////////////////////// MONGO \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

// Configuração do appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings")
);

// Instancia do context (substitui o AddDbContext<>())
builder.Services.AddSingleton<AppDbContext>();


/////////////////////////////// JWT \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var chaveSecreta = jwtSettings["Secret"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,           // true se quiser validar o emissor
        ValidateAudience = false,         // true se quiser validar o público
        ValidateLifetime = true,          // valida expiração
        ValidateIssuerSigningKey = true,  // valida a assinatura
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta!))
    };
});

//INSTANCIA JWT
builder.Services.AddSingleton<TokenService>();

///////////////////////////// CORS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

builder.Services.AddCors(options =>
{
    options.AddPolicy("MinhaPoliticaCors", policy =>
    {
        policy.WithOrigins("http://localhost:5028") // Origem que você quer permitir
              .AllowAnyHeader()                     // Permite qualquer cabeçalho
              .AllowAnyMethod();                    // Permite GET, POST, PUT, DELETE, etc.
    });
});

//////////////////////////// SERVICES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IGastoMensalService, GastoMensalService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProdutosService, ProdutosService>();
builder.Services.AddScoped<Files>();

///////////////////////////////////////////////////////////////////////

var app = builder.Build();

// Usar CORS antes dos endpoints
app.UseCors("MinhaPoliticaCors");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
