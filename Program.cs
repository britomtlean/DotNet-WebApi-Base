using System.Text;
using WebApi2026.Context;
using WebApi2026.Interfaces;
using WebApi2026.Services;
using WebApi2026.Settings;

// TOKEN
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

// STATIC
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;

//SOCKET
using WebApi2026.Hubs;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


////////////////////////////// MONGO \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

// Configuração do appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings")
);

// INSTANCIA CONTEXT
builder.Services.AddSingleton<AppDbContext>();


///////////////////////////// CORS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

builder.Services.AddCors(options =>
{
    options.AddPolicy("MinhaPoliticaCors", policy =>
    {
        policy.WithOrigins("http://localhost:5028") // Origem que você quer permitir
              .AllowAnyHeader()                     // Permite qualquer cabeçalho
              .AllowAnyMethod()                     // Permite GET, POST, PUT, DELETE, etc.
              .AllowCredentials();
    });
});


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


//////////////////////////// SIGNALR \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
builder.Services.AddSignalR();


//////////////////////////// INSTANCES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IGastoMensalService, GastoMensalService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProdutosService, ProdutosService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<FilesSettings>();
builder.Services.AddScoped<TokenSettings>();

///////////////////////////////////////////////////////////////////////

var app = builder.Build();

// Usar CORS antes dos endpoints
app.UseCors("MinhaPoliticaCors");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Rota do socket
app.MapHub<ChatHub>("/chat");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


//////////////////////////// PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

var provider = new FileExtensionContentTypeProvider();

provider.Mappings[".avif"] = "image/avif";

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(
            Directory.GetCurrentDirectory(),
            "Public",
            "Images"
        )
    ),
    RequestPath = "/images",
    ContentTypeProvider = provider
});

///////////////////////////////////////////////////////////////////////

app.MapControllers();

app.Run();
