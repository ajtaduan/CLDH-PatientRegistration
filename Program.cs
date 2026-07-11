using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using CLDH.PatientRegistration.Data;
using CLDH.PatientRegistration.Models;
using CLDH.PatientRegistration.Services;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// DB connection using SQlite
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// My auth service, for checking hashing/checking passwords
builder.Services.AddScoped<AuthService>();

// Cookie-based login, when user logs in ASP.NET sets a secure cookie
// and checks it on every request after that instead of me passing tokents manually
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.HttpOnly = true; // JS on the frontend cant read this cookie
    options.ExpireTimeSpan = TimeSpan.FromHours(8); // Auto logout after 8hrs idle
    options.SlidingExpiration = true;
    options.LoginPath = "/api/auth/login";
    options.Events.OnRedirectToLogin = context =>
    {
        // By default ASP.NET redirects unauthenticated API calls to login page
        // Since this is an API, i want to just return 401 instead
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Seed one admin account on startup if Users table is empty
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var authService = scope.ServiceProvider.GetRequiredService<AuthService>();

    if (!db.Users.Any())
    {
        var admin = new User { Username = "admin" };
        admin.PasswordHash = authService.HashPassword(admin, "Cldh@2026!");
        db.Users.Add(admin);
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseDefaultFiles(); //Serves index.html when visiting the root URL
app.UseStaticFiles(); // Serves everything inside wwwroot (html, css, js)

app.UseAuthentication(); // Checks who you are first
app.UseAuthorization(); // Then checks what you're allowed to do

app.MapControllers();

app.Run();
