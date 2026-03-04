using Microsoft.EntityFrameworkCore;
using TaskTracker.Web.Models;

namespace TaskTracker.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
}