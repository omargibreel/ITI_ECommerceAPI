using Common.Settings;
using Ecommerce.API.Extensions;
using Ecommerce.BLL;
using Ecommerce.BLL.Services.Classes;
using Ecommerce.BLL.Services.Interfaces;
using Ecommerce.DAL;
using Ecommerce.DAL.Data.Models;
using Ecommerce.DAL.UnitOfWork;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ── Paths ──────────────────────────────────────────────────────────
var wwwrootPath = builder.Environment.WebRootPath
    ?? Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
Directory.CreateDirectory(wwwrootPath);

// ── Settings ───────────────────────────────────────────────────────
builder.Services.Configure<ImageSettings>(options =>
{
    options.UploadBasePath = Path.Combine(wwwrootPath, "images");
    options.MaxFileSize = 5 * 1024 * 1024;
    options.AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
});
builder.Services.Configure<JwtSetting>(
    builder.Configuration.GetSection("JwtSettings"));

// ── Database ───────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Identity ───────────────────────────────────────────────────────
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 6;
    opt.Password.RequireUppercase = true;
    opt.Password.RequireDigit = true;
    opt.Password.RequireNonAlphanumeric = false;
    opt.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

// ── JWT Authentication ─────────────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSetting>()!;
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});

// ── CORS ───────────────────────────────────────────────────────────
builder.Services.AddCors(opt => opt.AddPolicy("AllowFrontend", policy =>
    policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
        ?? new[] { "http://localhost:3000" })
          .AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

// ── Repository / UoW ──────────────────────────────────────────────
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ── BLL Services ──────────────────────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

// ── AutoMapper ────────────────────────────────────────────────────
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfiles).Assembly);

// ── FluentValidation ──────────────────────────────────────────────
builder.Services.AddValidatorsFromAssembly(typeof(MappingProfiles).Assembly);
builder.Services.AddFluentValidationAutoValidation();


// ── MVC + Static Files ────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{ app.MapOpenApi(); app.MapScalarApiReference(); }

await app.SeedRolesAndUsersAsync();

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
