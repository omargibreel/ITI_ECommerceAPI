using Common.Image;
using Ecommerce.DAL;
using Ecommerce.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


var wwwrootPath = builder.Environment.WebRootPath
    ?? Path.Combine(builder.Environment.ContentRootPath, "wwwroot");

Directory.CreateDirectory(wwwrootPath);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(IGenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.Configure<ImageSettings>(options =>
{
    options.UploadBasePath = Path.Combine(wwwrootPath, "images");
    options.MaxFileSize = 5 * 1024 * 1024;
    options.AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
