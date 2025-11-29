using Finance.API.Data;
using Finance.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------
// 1. КОНФИГУРАЦИЯ БАЗЫ ДАННЫХ И IDENTITY (RBAC)
// -----------------------------------------------------------

// Получаем строку подключения из appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Регистрация DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Регистрация ASP.NET Identity для управления пользователями и ролями
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Настройки безопасности (можно сделать строже в реальном банковском проекте)
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<ApplicationDbContext>() // Привязываем Identity к нашему DbContext
    .AddDefaultTokenProviders();


// -----------------------------------------------------------
// 2. КОНФИГУРАЦИЯ JWT AUTHENTICATION 
// -----------------------------------------------------------

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


// -----------------------------------------------------------
// 3. КОНФИГУРАЦИЯ CORS (для связи с React)
// -----------------------------------------------------------

// В Development режиме разрешаем доступ с любого домена (для React на другом порту)
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .WithOrigins(builder.Configuration["FrontendUrl"] ?? "http://localhost:3000") // Предполагаем, что React будет на порту 3000
              .AllowCredentials(); // Важно для аутентификации
    });
});


// -----------------------------------------------------------
// 4. ДОБАВЛЕНИЕ СЕРВИСОВ MVC/SWAGGER
// -----------------------------------------------------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


// -----------------------------------------------------------
// 5. НАСТРОЙКА HTTP-КОНВЕЙЕРА (MIDDLEWARE)
// -----------------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ОБЯЗАТЕЛЬНО: Включаем CORS перед Authentication и Authorization
app.UseCors("CorsPolicy");

// ОБЯЗАТЕЛЬНО: Включаем Authentication перед Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();