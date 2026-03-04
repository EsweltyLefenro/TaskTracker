using Microsoft.EntityFrameworkCore;
using TaskTracker.Web.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// DB
if (builder.Environment.IsEnvironment("Testing"))
{
    // Для dotnet test — не лезем в реальный Postgres
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseInMemoryDatabase("TaskTracker_Test"));
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseNpgsql(connectionString));
}

var app = builder.Build();

// Миграции нужны для обычного запуска/докера, но НЕ для тестов
if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapRazorPages();
app.Run();

public partial class Program { }
